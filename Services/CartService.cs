using System.Text.Json;
using Belajardotnet.Models;
using Microsoft.AspNetCore.Http;

namespace Belajardotnet.Services;

public class CartService(IHttpContextAccessor httpContextAccessor)
{
    private const string CartKey = "katalogku-cart";
    private ISession Session => httpContextAccessor.HttpContext!.Session;

    public List<CartItem> GetItems() => Session.GetString(CartKey) is { } json
        ? JsonSerializer.Deserialize<List<CartItem>>(json) ?? []
        : [];

    public void Save(List<CartItem> items) => Session.SetString(CartKey, JsonSerializer.Serialize(items));

    public void Add(Product product)
    {
        var items = GetItems();
        var item = items.FirstOrDefault(item => item.ProductId == product.Id);
        if (item is null)
        {
            items.Add(new CartItem { ProductId = product.Id, Name = product.Name, ImagePath = product.ImagePath, Price = product.Price, Quantity = 1 });
        }
        else
        {
            item.Quantity++;
        }
        Save(items);
    }

    public void Remove(int productId) => Save(GetItems().Where(item => item.ProductId != productId).ToList());

    public void Update(int productId, int quantity)
    {
        var items = GetItems();
        var item = items.FirstOrDefault(item => item.ProductId == productId);
        if (item is null) return;
        if (quantity <= 0) Remove(productId);
        else { item.Quantity = quantity; Save(items); }
    }

    public void Clear() => Session.Remove(CartKey);
}
