using Belajardotnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Belajardotnet.Pages;

[Authorize(Roles = "User")]
public class CartModel(CartService cart) : PageModel
{
    public IList<Models.CartItem> Items { get; private set; } = [];
    public decimal Subtotal => Items.Sum(item => item.Total);
    public void OnGet() => Items = cart.GetItems();
    public IActionResult OnPostUpdate(int id, int quantity) { cart.Update(id, quantity); return RedirectToPage(); }
    public IActionResult OnPostRemove(int id) { cart.Remove(id); return RedirectToPage(); }
}
