using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Localization;
using MudBlazor;
using MudBlazor.Services;
using MudExtensions.Services;
using SpellChecker;
using SpellChecker.Classes;
using SpellChecker.Core.Interfaces;
using SpellChecker.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<LayoutService>();

builder.Services.AddMudServices(config => {
	                                config.SnackbarConfiguration.PreventDuplicates = true;
	                                config.SnackbarConfiguration.NewestOnTop = true;
	                                config.SnackbarConfiguration.ShowCloseIcon = true;
	                                config.SnackbarConfiguration.VisibleStateDuration = 5000;
	                                config.SnackbarConfiguration.HideTransitionDuration = 250;
	                                config.SnackbarConfiguration.ShowTransitionDuration = 250;
	                                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
	                                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
                                });

builder.Services.AddMudExtensions();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
builder.Services.AddSingleton<IStringLocalizer>(provider => {
	                                                var factory = provider.GetRequiredService<IStringLocalizerFactory>();
	                                                return factory.Create("Localization", "SpellChecker");
                                                });

builder.Services.AddScoped<CultureProviderService>();
builder.Services.AddScoped<IUserPreferencesService, UserPreferencesService>();


var build = builder.Build();

await build.SetAppCulture();

await build.RunAsync();
