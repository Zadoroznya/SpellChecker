using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using SpellChecker.Server.Models;

namespace SpellChecker.Server.Helpers;

public class DbInitializer {
	public DbInitializer(ILogger logger, AppDbContext dbContext) {
		Logger = logger;
		DbContext = dbContext;

		if (dbContext.Database.GetService<IDatabaseCreator>() is not RelationalDatabaseCreator databaseCreator) {
			Logger.Log(LogLevel.Error, "Unknown error occurred while Database initialize");
			return;
		}

		if (!databaseCreator.ExistsAsync()
		                    .GetAwaiter()
		                    .GetResult()) {
			logger.LogWarning("Database not exist. Creating database");
			dbContext.CreateDatabase()
			         .GetAwaiter()
			         .GetResult();
			logger.LogInformation("Database created");
		}

		if (!databaseCreator.HasTables()) {
			logger.LogWarning("Database has not tables. Creating tables");
			dbContext.CreateDatabase()
			         .GetAwaiter()
			         .GetResult();
			logger.LogInformation("Tables created");
		}

		CheckConnect();
	}

	private static ILogger? Logger { get; set; }
	private static AppDbContext? DbContext { get; set; }
	public bool IsConnected { get; set; }

	private void CheckConnect() {
		if (DbContext == null) {
			Logger?.Log(LogLevel.Error, $"\"{nameof(AppDbContext)}\" is null");
			return;
		}

		IsConnected = DbContext.Database.CanConnect();
		if (IsConnected)
			Logger?.Log(LogLevel.Information, "Database connected");
		else
			Logger?.Log(LogLevel.Error, "Database is not connected");
	}
}