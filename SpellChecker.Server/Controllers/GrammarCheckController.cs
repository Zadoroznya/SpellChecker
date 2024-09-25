using Microsoft.AspNetCore.Mvc;
using SpellChecker.Core.Interfaces;
using SpellChecker.Core.Models;

namespace SpellChecker.Server.Controllers;

[ApiController]
[Route("api")]
public class GrammarCheckController : ControllerBase {
	private readonly IGrammarChecker _grammarCheckerService;

	public GrammarCheckController(IGrammarChecker grammarCheckerService) {
		_grammarCheckerService = grammarCheckerService;
	}

	[HttpPost("Check")]
	public async Task<IActionResult> Check(GrammarCheckRequest request) {
		try {
			if (string.IsNullOrWhiteSpace(request.PromptRequest))
				return BadRequest();

			return Ok(await _grammarCheckerService.CheckGrammarAsync(request.PromptRequest));
		}
		catch (Exception e) {
			return BadRequest(new GrammarCheckResponse { ErrorMessage = e.ToString() });
		}
	}

	[HttpDelete("DeleteItem/{id:guid}")]
	public async Task<IActionResult> DeleteItem(Guid id) {
		try {
			return Ok(await _grammarCheckerService.DeleteGrammarItemAsync(id));
		}
		catch (Exception e) {
			return BadRequest(new GrammarCheckResponse { ErrorMessage = e.ToString() });
		}
	}

	[HttpDelete("DeleteHistory")]
	public async Task<IActionResult> DeleteHistory() {
		try {
			return Ok(await _grammarCheckerService.DeleteGrammarHistoryAsync());
		}
		catch (Exception e) {
			return BadRequest(new GrammarCheckResponse { ErrorMessage = e.ToString() });
		}
	}

	[HttpGet("GetHistory")]
	public async Task<IActionResult> GetHistory() {
		try {
			return Ok(await _grammarCheckerService.GetHistoryAsync());
		}
		catch (Exception e) {
			return NotFound(new GrammarCheckResponse { ErrorMessage = e.ToString() });
		}
	}
}