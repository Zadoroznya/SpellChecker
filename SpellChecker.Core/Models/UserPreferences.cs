using SpellChecker.Core.Enums;

namespace SpellChecker.Core.Models;

public class UserPreferences {
	public AppLanguage Language { get; set; } = AppLanguage.En;

	public ThemeMode Theme { get; set; }
}