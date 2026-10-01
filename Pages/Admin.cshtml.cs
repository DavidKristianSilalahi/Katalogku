using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Belajardotnet.Pages;

[Authorize(Roles = "Admin")]
public class AdminModel : PageModel
{
    public IActionResult OnGet() => RedirectToPage("/Index");
}
