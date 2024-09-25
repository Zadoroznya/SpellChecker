using Microsoft.EntityFrameworkCore;
using OpenAI_API;
using OpenAI_API.Chat;
using SpellChecker.Core.Interfaces;
using SpellChecker.Core.Models;
using SpellChecker.Server.Helpers;
using SpellChecker.Server.Models;

namespace SpellChecker.Server.Services;

public class GrammarCheckerService : IGrammarChecker {
	private readonly AppDbContext _appDbContext;
	private readonly ILogger<GrammarCheckerService> _logger;
	private readonly OpenAIAPI _openAiApi;

	public GrammarCheckerService(IConfiguration configuration, AppDbContext appDbContext, ILogger<GrammarCheckerService> logger) {
		_appDbContext = appDbContext;
		_logger = logger;

		var apiKey = configuration["OpenAIKey"];
		_openAiApi = new OpenAIAPI(apiKey);
	}

	public async Task<GrammarCheckResponse> CheckGrammarAsync(string? prompt) {
		try {
			if (string.IsNullOrWhiteSpace(prompt)) {
				const string message = $"\"{nameof(prompt)}\" is null or empty";
				_logger.Log(LogLevel.Error, message);
				return new GrammarCheckResponse { ErrorMessage = message };
			}

			var models = await _openAiApi.Models.GetModelsAsync();
			var model = models.Find(model => model.ModelID == "gpt-4o");

			var chatRequest = new ChatRequest {
				Model = model,
				Messages = new List<ChatMessage> {
					new(ChatMessageRole.System, "Виправляй лише граматичні помилки в тексті. Нічого не додавай. А якщо все написано правильно, тоді просто верни такий же текст"),
					new(ChatMessageRole.User, prompt)
				}
			};
			var result = await _openAiApi.Chat.CreateChatCompletionAsync(chatRequest);

			var correctedText = string.Empty;
			if (result.Choices.Any())
				correctedText = result.Choices.FirstOrDefault()
				                      ?.Message.TextContent;

			if (string.IsNullOrWhiteSpace(correctedText))
				correctedText = $"Не змогло застосувати виправлення для \"{prompt}\"";

			await _appDbContext.GrammarCheckHistories.AddAsync(new GrammarCheckHistory {
				OriginalText = prompt,
				CorrectedText = correctedText,
				CheckedAt = DateTime.Now
			});

			await _appDbContext.SaveChangesAsync();

			return new GrammarCheckResponse { CorrectedText = correctedText };
		}
		catch (Exception e) {
			_logger.Log(LogLevel.Error, e.ToString());
			return new GrammarCheckResponse { ErrorMessage = e.ToString() };
		}
	}

	public async Task<bool> DeleteGrammarItemAsync(Guid? id) {
		try {
			var item = await _appDbContext.GrammarCheckHistories.FindAsync(id);
			if (item != null)
				_appDbContext.GrammarCheckHistories.Remove(item);

			await _appDbContext.SaveChangesAsync();
			_logger.Log(LogLevel.Information, "History item deleted");
			return true;
		}
		catch (Exception e) {
			_logger.Log(LogLevel.Error, e.ToString());
			return false;
		}
	}

	public async Task<bool> DeleteGrammarHistoryAsync() {
		try {
			_appDbContext.GrammarCheckHistories.Clear();
			await _appDbContext.SaveChangesAsync();
			
			_logger.Log(LogLevel.Information, "History deleted");
			return true;
		}
		catch (Exception e) {
			_logger.Log(LogLevel.Error, e.ToString());
			return false;
		}
	}

	public async Task<GrammarCheckResponse> GetHistoryAsync() {
		try {
			var histories = new List<GrammarCheckHistory>();
			if (_appDbContext.GrammarCheckHistories.Any())
				histories = await _appDbContext.GrammarCheckHistories.ToListAsync();

			return new GrammarCheckResponse { Histories = histories };
		}
		catch (Exception e) {
			_logger.Log(LogLevel.Error, e.ToString());
			return new GrammarCheckResponse { ErrorMessage = e.ToString() };
		}
	}
}