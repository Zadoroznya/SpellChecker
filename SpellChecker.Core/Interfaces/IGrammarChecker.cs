using SpellChecker.Core.Models;

namespace SpellChecker.Core.Interfaces;

public interface IGrammarChecker {
	/// <summary>
	/// Виконує функціонал перевірки граматики
	/// </summary>
	/// <param name="prompt">Текст, що введений користувачем</param>
	/// <returns>Результат обробки даних</returns>
	Task<GrammarCheckResponse> CheckGrammarAsync(string? prompt);

	/// <summary>
	/// Видаляє елемент історії перевірки граматики
	/// </summary>
	/// <param name="id">Ідентифікатор елементу історії</param>
	/// <returns>Результат видалення [Так/Ні]</returns>
	Task<bool> DeleteGrammarItemAsync(Guid? id);

	/// <summary>
	/// Видаляє всю історію перевірки граматики
	/// </summary>
	/// <returns>Результат видалення [Так/Ні]</returns>
	Task<bool> DeleteGrammarHistoryAsync();

	/// <summary>
	/// Виконує запит на отримання списку історії перевірки граматики
	/// </summary>
	/// <returns>Список історії перевірки граматики</returns>
	Task<GrammarCheckResponse> GetHistoryAsync();
}