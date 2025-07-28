using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContactCalls.Pages;

public class ContactsPageModel : PageModel
{
    private readonly ILogger<ContactsPageModel> _logger;

    public ContactsPageModel(ILogger<ContactsPageModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}