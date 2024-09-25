using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace SpellChecker.Layout;

public partial class MainLayout : LayoutComponentBase, IDisposable {
	private MudThemeProvider? _mudThemeProvider;

	public void Dispose() {
		LayoutService.MajorUpdateOccurred -= LayoutServiceOnMajorUpdateOccured!;
	}

	protected override void OnInitialized() {
		LayoutService.MajorUpdateOccurred += LayoutServiceOnMajorUpdateOccured!;
		base.OnInitialized();
	}

	protected override async Task OnAfterRenderAsync(bool firstRender) {
		await base.OnAfterRenderAsync(firstRender);

		if (firstRender) {
			await ApplyUserPreferences();
			if (_mudThemeProvider != null)
				await _mudThemeProvider.WatchSystemPreference(OnSystemPreferenceChanged);
			StateHasChanged();
		}
	}

	private async Task ApplyUserPreferences() {
		var defaultDarkMode = _mudThemeProvider != null && await _mudThemeProvider.GetSystemPreference();
		await LayoutService.ApplyThemeMode(defaultDarkMode);
	}

	private async Task OnSystemPreferenceChanged(bool newValue) =>
		await LayoutService.OnSystemPreferenceChanged(newValue);

	private void LayoutServiceOnMajorUpdateOccured(object sender, EventArgs e) => StateHasChanged();
}