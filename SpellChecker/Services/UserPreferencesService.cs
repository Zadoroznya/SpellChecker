using Blazored.LocalStorage;
using SpellChecker.Classes;
using SpellChecker.Core.Interfaces;
using SpellChecker.Core.Models;

namespace SpellChecker.Services;

public class UserPreferencesService(ILocalStorageService localStorage) : IUserPreferencesService {

	public async Task SaveUserPreferences(UserPreferences userPreferences) =>
		await localStorage.SetItemAsync(Constants.UserPreferences, userPreferences);

	public async Task<UserPreferences> Initialize() {
		var value = new UserPreferences();
		await localStorage.SetItemAsync(Constants.UserPreferences, value);
		return value;
	}

	public async Task<UserPreferences> LoadUserPreferences() {
		var preferences =  await localStorage.GetItemAsync<UserPreferences>(Constants.UserPreferences);
		return preferences ?? await Initialize();
	}
}