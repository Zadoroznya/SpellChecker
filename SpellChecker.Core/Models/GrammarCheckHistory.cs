using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpellChecker.Core.Models;

public class GrammarCheckHistory {
	[Key] public Guid Id { get; set; }
	public string? OriginalText { get; set; }
	public string? CorrectedText { get; set; }
	[NotMapped] public bool IsCorrect => string.Equals(OriginalText, CorrectedText);
	public DateTime CheckedAt { get; set; }
}