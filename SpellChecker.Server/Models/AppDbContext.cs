using Microsoft.EntityFrameworkCore;
using SpellChecker.Core.Models;

namespace SpellChecker.Server.Models;

public class AppDbContext : DbContext {
	public DbSet<GrammarCheckHistory> GrammarCheckHistories { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
		optionsBuilder.UseSqlite("Data Source=GrammarCheck.db");

	public async Task<bool> CreateDatabase() => await Database.EnsureCreatedAsync();
}