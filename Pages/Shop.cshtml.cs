using Belajardotnet.Data;
using Belajardotnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Belajardotnet.Pages;

[Authorize(Roles = "User")]
public class ShopModel(AppDbContext database, CartService cart) : PageModel
{
    public IList<Models.Product> Products { get; private set; } = [];
    public async Task OnGetAsync() => Products = await database.Products.OrderByDescending(product => product.CreatedAt).ToListAsync();

    public async Task<IActionResult> OnPostAddAsync(int id)
    {
        var product = await database.Products.FindAsync(id);
        if (product is not null && product.Stock > 0) { cart.Add(product); TempData["Message"] = $"{product.Name} masuk ke keranjang."; }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostOrderAsync(int id)
    {
        var product = await database.Products.FindAsync(id);
        if (product is not null && product.Stock > 0)
        {
            cart.Add(product);
            return RedirectToPage("/Checkout");
        }

        TempData["Message"] = "Produk sedang tidak tersedia.";
        return RedirectToPage();
    }
}
