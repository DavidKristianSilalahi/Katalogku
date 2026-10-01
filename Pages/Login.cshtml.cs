using System.Security.Claims;
using Belajardotnet.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Belajardotnet.Pages;

public class LoginModel(AppDbContext database) : PageModel
{
    [BindProperty] public string Username { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;
    [BindProperty(SupportsGet = true)] public string? ReturnUrl { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = database.Users.FirstOrDefault(item => item.Username == Username.Trim() && item.Password == Password);
        if (user is null)
        {
            ErrorMessage = "Username atau password belum tepat.";
            return Page();
        }

        var claims = new[] { new Claim(ClaimTypes.Name, user.Username), new Claim(ClaimTypes.Role, user.Role) };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));
        return LocalRedirect(string.IsNullOrWhiteSpace(ReturnUrl) ? (user.Role == "Admin" ? "/Admin" : "/Shop") : ReturnUrl);
    }
}
