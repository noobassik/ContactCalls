using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContactCalls.Pages;

public class ReportsPageModel : PageModel
{
	private readonly ILogger<ReportsPageModel> _logger;

	public ReportsPageModel(ILogger<ReportsPageModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}