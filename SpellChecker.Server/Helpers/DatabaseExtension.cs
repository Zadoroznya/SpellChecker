using Microsoft.EntityFrameworkCore;
using SpellChecker.Core.Models;

namespace SpellChecker.Server.Helpers;

public static class DatabaseExtension {
	public static void Clear<T>(this DbSet<T> dbSet) where T : class => dbSet.RemoveRange(dbSet);
}