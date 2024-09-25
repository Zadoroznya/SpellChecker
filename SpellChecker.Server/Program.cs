using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.Extensions.Logging;
using SpellChecker.Core.Interfaces;
using SpellChecker.Server.Helpers;
using SpellChecker.Server.Helpers.Log;
using SpellChecker.Server.Models;
using SpellChecker.Server.Services;

var builder = WebApplication.CreateBuilder(args);

var provider = new FileLoggerProvider(Path.Combine(Directory.GetCurrentDirectory(), "logs"));
var logger = provider.CreateLogger("Server");

try {
	builder.Services.AddLogging(loggingBuilder =>
		                            loggingBuilder.AddProvider(new FileLoggerProvider(Path.Combine(Directory.GetCurrentDirectory(),
			                                                                              "logs"))));
	logger.Log(LogLevel.Information, "Add logging provider");

	//builder.Services.AddCors(options => {
	//	                         options.AddPolicy("CorsPolicy",
	//	                                           corsPolicyBuilder => corsPolicyBuilder
	//	                                                                .AllowAnyOrigin()
	//	                                                                .AllowAnyMethod()
	//	                                                                .AllowAnyHeader());
	//                         });

	builder.Services.AddControllers();
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();
	builder.Services.AddRazorPages();
	builder.Services.AddControllersWithViews();
	builder.Services.AddEntityFrameworkSqlite();
	builder.Services.AddDbContext<AppDbContext>();

	builder.Services.AddScoped<IGrammarChecker, GrammarCheckerService>();
	
	StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

	logger.Log(LogLevel.Information, "Build success");

	var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment()) {
		app.UseWebAssemblyDebugging();
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	//app.UseCors("CorsPolicy");

	app.UseHttpsRedirection();
	app.UseBlazorFrameworkFiles();

	app.UseStaticFiles();
	app.MapRazorPages();
	app.MapControllers();
	app.UseRouting();
	app.MapFallbackToFile("index.html", new StaticFileOptions());

	using var scope = app.Services.CreateScope();
	var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

	var dbInitializer = new DbInitializer(logger, dbContext);
	if (!dbInitializer.IsConnected) {
		logger.Log(LogLevel.Error, "Not connected to database for check or initialize");
		return;
	}

	app.Run();
}
catch (Exception e) {
	logger.Log(LogLevel.Error, e.ToString());
}