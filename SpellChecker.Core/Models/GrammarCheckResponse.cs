namespace SpellChecker.Core.Models;

public class GrammarCheckResponse {
	public string? CorrectedText { get; set; } = null;
	public List<GrammarCheckHistory>? Histories { get; set; } = [];
	public string? ErrorMessage { get; set; } = null;
}