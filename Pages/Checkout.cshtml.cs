using Belajardotnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Belajardotnet.Pages;

[Authorize(Roles = "User")]
public class CheckoutModel(CartService cart) : PageModel
{
    public IList<Models.CartItem> Items { get; private set; } = [];
    public decimal Total => Items.Sum(item => item.Total);
    [BindProperty] public string Name { get; set; } = string.Empty;
    [BindProperty] public string Address { get; set; } = string.Empty;
    [BindProperty] public string Payment { get; set; } = "Transfer bank";
    public void OnGet() => Items = cart.GetItems();
    public IActionResult OnPost()
    {
        Items = cart.GetItems();
        if (Items.Count == 0) return RedirectToPage("/Shop");
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Address)) { ModelState.AddModelError(string.Empty, "Nama dan alamat wajib diisi."); return Page(); }
        cart.Clear();
        TempData["Message"] = "Pesanan berhasil dibuat. Terima kasih sudah berbelanja.";
        return RedirectToPage("/Shop");
    }
}
