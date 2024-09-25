using System.Globalization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SpellChecker.Core.Interfaces;

namespace SpellChecker.Classes;

public static class WebAssemblyHostExtension {
	public static async Task SetAppCulture(this WebAssemblyHost host) {
		var userPreferencesService = host.Services.GetRequiredService<IUserPreferencesService>();
		var preferences = await userPreferencesService.LoadUserPreferences();

		var culture = preferences.Language.GetCulture();

		CultureInfo.DefaultThreadCurrentCulture = culture;
		CultureInfo.DefaultThreadCurrentUICulture = culture;
	}
}