using MudBlazor;
using SpellChecker.Core.Enums;
using SpellChecker.Core.Interfaces;
using SpellChecker.Core.Models;

namespace SpellChecker.Services;

public class LayoutService(IUserPreferencesService userPreferencesService) {
	private bool _systemPreferences;
	private UserPreferences? _userPreferences;

	public ThemeMode CurrentDarkLightMode { get; private set; } = ThemeMode.System;

	public bool IsDarkMode { get; private set; }

	public bool ObserveSystemThemeChange { get; private set; }

	public MudTheme? CurrentTheme { get; private set; }

	public void SetDarkMode(bool value) {
		IsDarkMode = value;
	}

	public async Task ApplyThemeMode(bool isDarkModeDefaultTheme) {
		_systemPreferences = isDarkModeDefaultTheme;
		_userPreferences = await userPreferencesService.LoadUserPreferences();

		CurrentDarkLightMode = _userPreferences.Theme;
		IsDarkMode = CurrentDarkLightMode switch {
			ThemeMode.Dark => true,
			ThemeMode.Light => false,
			ThemeMode.System => isDarkModeDefaultTheme,
			_ => IsDarkMode
		};
	}

	public Task OnSystemPreferenceChanged(bool newValue) {
		_systemPreferences = newValue;

		if (CurrentDarkLightMode == ThemeMode.System) {
			IsDarkMode = newValue;
			OnMajorUpdateOccurred();
		}

		return Task.CompletedTask;
	}

	public event EventHandler? MajorUpdateOccurred;

	private void OnMajorUpdateOccurred() => MajorUpdateOccurred?.Invoke(this, EventArgs.Empty);

	public async Task SetTheme(MudTheme? theme) {
		CurrentTheme = theme;
		if (_userPreferences != null) {
			_userPreferences.Theme = CurrentDarkLightMode;
			await userPreferencesService.SaveUserPreferences(_userPreferences);
		}

		OnMajorUpdateOccurred();
	}
}