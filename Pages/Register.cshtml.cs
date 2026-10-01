using Belajardotnet.Data;
using Belajardotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Belajardotnet.Pages;

public class RegisterModel(AppDbContext database) : PageModel
{
    [BindProperty] public string Username { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;
    [BindProperty] public string ConfirmPassword { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Username.Trim().Length < 3 || Password.Length < 6)
        {
            ErrorMessage = "Username minimal 3 karakter dan password minimal 6 karakter.";
            return Page();
        }
        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Konfirmasi password belum sama.";
            return Page();
        }
        if (database.Users.Any(user => user.Username == Username.Trim()))
        {
            ErrorMessage = "Username tersebut sudah digunakan.";
            return Page();
        }
        database.Users.Add(new User { Username = Username.Trim(), Password = Password, Role = "User" });
        await database.SaveChangesAsync();
        TempData["Message"] = "Akun berhasil dibuat. Silakan masuk.";
        return RedirectToPage("/Login");
    }
}
