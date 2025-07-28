using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContactCalls.Pages;

public class ContactDetailPageModel : PageModel
{
	private readonly ILogger<ContactDetailPageModel> _logger;

	public ContactDetailPageModel(ILogger<ContactDetailPageModel> logger)
	{
		_logger = logger;
	}

	public void OnGet(int id)
	{
	}
}