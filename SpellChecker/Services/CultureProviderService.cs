using SpellChecker.Core.Enums;
using SpellChecker.Core.Interfaces;

namespace SpellChecker.Services;

public class CultureProviderService(IUserPreferencesService userPreferencesService) {
	public async Task SwitchCultureAsync(AppLanguage language) {
		var userPreferences = await userPreferencesService.LoadUserPreferences();
		userPreferences.Language = language;
		await userPreferencesService.SaveUserPreferences(userPreferences);
	}

	public async Task<AppLanguage> GetSavedCultureAsync() {
		var preferences = await userPreferencesService.LoadUserPreferences();
		return preferences.Language;
	}
}