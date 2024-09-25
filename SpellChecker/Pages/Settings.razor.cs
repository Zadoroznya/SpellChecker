using SpellChecker.Core.Enums;
using SpellChecker.Core.Models;

namespace SpellChecker.Pages;

public partial class Settings {
	private ThemeMode _themeMode = ThemeMode.System;
	private AppLanguage _selectedLanguage = AppLanguage.En;

	protected override async Task OnInitializedAsync() {
		var preferences = await PreferencesService.LoadUserPreferences();
		_themeMode = preferences.Theme;
		_selectedLanguage = preferences.Language;
	}

	private async Task SaveSettings() {
		await PreferencesService.SaveUserPreferences(new UserPreferences {
			Language = _selectedLanguage,
			Theme = _themeMode
		});

		var mode = _themeMode switch {
			ThemeMode.System => LayoutService.IsDarkMode,
			ThemeMode.Light => false,
			ThemeMode.Dark => true,
			_ => LayoutService.IsDarkMode
		};

		await LayoutService.ApplyThemeMode(mode);
		Navigation.NavigateTo(Navigation.Uri, forceLoad: true);
	}
}