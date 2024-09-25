using SpellChecker.Core.Models;

namespace SpellChecker.Core.Interfaces;

public interface IUserPreferencesService {
	/// <summary>
	///     Зберігає UserPreferences у локальному сховищі
	/// </summary>
	/// <param name="userPreferences">UserPreferences для збереження в локальному сховищі</param>
	public Task SaveUserPreferences(UserPreferences userPreferences);

	/// <summary>
	///     Завантажує UserPreferences у локальне сховище
	/// </summary>
	/// <returns>Об'єкт UserPreferences. Створюється за замовчуванням, якщо не знайдено жодних налаштувань</returns>
	public Task<UserPreferences> LoadUserPreferences();
}