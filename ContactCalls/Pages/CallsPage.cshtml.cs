using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContactCalls.Pages;

public class CallsPageModel : PageModel
{
	private readonly ILogger<CallsPageModel> _logger;

	public CallsPageModel(ILogger<CallsPageModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}