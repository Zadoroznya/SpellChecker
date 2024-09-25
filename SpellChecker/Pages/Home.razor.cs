using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using MudBlazor;
using SpellChecker.Classes;
using SpellChecker.Core.Models;

namespace SpellChecker.Pages;

public partial class Home {
	private bool IsDisableCheck => IsLoading || string.IsNullOrWhiteSpace(Prompt) || Prompt.Length <= 9;
	private string? Prompt { get; set; }
	private GrammarCheckResponse? Response { get; set; }
	private bool IsLoading { get; set; }
	private string? OriginalText { get; set; }
	private string? FixedText { get; set; }

	private async Task CheckGrammar() {
		IsLoading = true;
		StateHasChanged();

		try {
			var content = JsonSerializer.Serialize(new GrammarCheckRequest { PromptRequest = Prompt },
			                                       new JsonSerializerOptions(JsonSerializerDefaults.Web) {
				                                       Encoder = JavaScriptEncoder.Default,
				                                       WriteIndented = true
			                                       });
			var responseMessage = await Http.PostAsync("api/Check",
			                                           new StringContent(content,
			                                                             Encoding.UTF8,
			                                                             MediaTypeNames.Application.Json));

			Response = await responseMessage.Content.ReadFromJsonAsync<GrammarCheckResponse>();

			if (Response != null) {
				if (string.IsNullOrWhiteSpace(Response.ErrorMessage)) {
					var diff = InlineDiffBuilder.Diff(Prompt, Response.CorrectedText);
					var originalBuilder = new StringBuilder();
					var fixedBuilder = new StringBuilder();
					foreach (var line in diff.Lines)
						switch (line.Type) {
							case ChangeType.Inserted:
								fixedBuilder.AppendLine(line.Text);
								break;
							case ChangeType.Deleted:
								originalBuilder.AppendLine(line.Text);
								break;
							case ChangeType.Unchanged:
								originalBuilder.AppendLine(line.Text);
								fixedBuilder.AppendLine(line.Text);
								break;
							case ChangeType.Imaginary:
							case ChangeType.Modified:
							default:
								originalBuilder.AppendLine(line.Text);
								fixedBuilder.AppendLine(line.Text);
								break;
						}

					OriginalText = originalBuilder.ToString();
					FixedText = fixedBuilder.ToString();
				}
				else {
					SnackbarService.Add(Response.ErrorMessage, Severity.Error);
				}
			}

			if (Response == null) {
				SnackbarService.Add(Localizer[Captions.ResponseError], Severity.Error);
				return;
			}

			if (!string.IsNullOrWhiteSpace(Response?.ErrorMessage))
				SnackbarService.Add(Response.ErrorMessage, Severity.Error);
		}
		catch (Exception ex) {
			SnackbarService.Add(ex.ToString(), Severity.Error);
		}
		finally {
			IsLoading = false;
			StateHasChanged();
		}
	}

	private void Reset() {
		Response = null;
		OriginalText = null;
		FixedText = null;
		Prompt = null;
		StateHasChanged();
	}
}