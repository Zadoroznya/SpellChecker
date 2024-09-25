using System.Net.Http.Json;
using System.Text.Json;
using MudBlazor;
using SpellChecker.Classes;
using SpellChecker.Components;
using SpellChecker.Core.Models;

namespace SpellChecker.Pages;

public partial class History {
	public bool IsLoading { get; set; } = true;
	public bool IsDisableClear => Histories.Count <= 0;
	public List<GrammarCheckHistory> Histories { get; set; } = [];

	private async Task ClearGrammar() {
		var dialog = await Dialog.ShowAsync<DeleteRequestDialog>(Localizer[Captions.ClearHistoryTitle], new DialogOptions {
			BackdropClick = false
		});
		var result = await dialog.Result;
		if (result is not { Canceled: true }) {
			var isDeleted = await Http.DeleteFromJsonAsync<bool>("api/DeleteHistory");
			if (isDeleted) {
				Histories.Clear();
				StateHasChanged();
			}
		}
	}

	protected override async Task OnInitializedAsync() {
		IsLoading = true;
		StateHasChanged();

		try {
			var response = await Http.GetFromJsonAsync<GrammarCheckResponse>("api/GetHistory",
			                                                                 new JsonSerializerOptions(JsonSerializerDefaults.Web));

			if (response is { Histories.Count: > 0 })
				Histories = response.Histories
				                    .OrderByDescending(history => history.CheckedAt)
				                    .ToList();
		}
		catch (Exception ex) {
			SnackbarService.Add($"{Localizer[Captions.HistoryLoadError]}: {ex}", Severity.Error);
		}
		finally {
			IsLoading = false;
			StateHasChanged();
		}
	}

	private async Task DeleteItem(GrammarCheckHistory item) {
		var dialog = await Dialog.ShowAsync<DeleteRequestDialog>(Localizer[Captions.DeleteHistoryItem], new DialogOptions
		{
			BackdropClick = false
		});
		var result = await dialog.Result;
		if (result is not { Canceled: true }) {
			var isSuccess = await Http.DeleteFromJsonAsync<bool>($"api/DeleteItem/{item.Id}");
			if (isSuccess) {
				var historyItem = Histories.Find(history => history.Id == item.Id);
				if (historyItem != null) {
					Histories.Remove(historyItem);
					StateHasChanged();
				}
			}

			var severity = isSuccess ? Severity.Success : Severity.Error;
			SnackbarService.Add(isSuccess
				                    ? Localizer[Captions.DeleteHistoryItemSuccess]
				                    : Localizer[Captions.DeleteHistoryItemError],
			                    severity);
		}
	}
}