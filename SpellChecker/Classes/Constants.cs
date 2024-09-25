using System.Globalization;
using SpellChecker.Core.Enums;

namespace SpellChecker.Classes;

public static class Constants {
	public const string AppName = nameof(SpellChecker);
	public const string UserPreferences = nameof(UserPreferences);

	public const string EnglishCultureName = "en-US";
	public const string UkrainianCultureName = "uk-UA";
	public const string RussianCultureName = "ru-RU";

	public static CultureInfo GetCulture(this AppLanguage language) {
		return language switch {
			AppLanguage.En => CultureInfo.GetCultureInfo(EnglishCultureName),
			AppLanguage.Ua => CultureInfo.GetCultureInfo(UkrainianCultureName),
			AppLanguage.Ru => CultureInfo.GetCultureInfo(RussianCultureName),
			_ => CultureInfo.GetCultureInfo(EnglishCultureName)
		};
	}
}