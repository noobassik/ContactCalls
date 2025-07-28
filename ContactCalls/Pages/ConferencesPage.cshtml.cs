using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContactCalls.Pages;

public class ConferencesPageModel : PageModel
{
	private readonly ILogger<ConferencesPageModel> _logger;

	public ConferencesPageModel(ILogger<ConferencesPageModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}