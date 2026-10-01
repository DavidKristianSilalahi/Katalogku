using Belajardotnet.Data;
using Belajardotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Belajardotnet.Pages;

[Authorize(Roles = "Admin")]
public class EditModel : PageModel
{
    private readonly AppDbContext _database;
    private readonly IWebHostEnvironment _environment;

    public EditModel(AppDbContext database, IWebHostEnvironment environment)
    {
        _database = database;
        _environment = environment;
    }

    [BindProperty]
    public Product Product { get; set; } = new();

    [BindProperty]
    public IFormFile? ImageFile { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var product = await _database.Products.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        Product = product;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ValidateImage())
        {
            return Page();
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var existingProduct = await _database.Products.FindAsync(Product.Id);
        if (existingProduct is null)
        {
            return NotFound();
        }

        existingProduct.Name = Product.Name;
        existingProduct.Category = Product.Category;
        existingProduct.Price = Product.Price;
        existingProduct.Stock = Product.Stock;
        if (ImageFile is not null)
        {
            DeleteImage(existingProduct.ImagePath);
            existingProduct.ImagePath = await SaveImageAsync(ImageFile);
        }

        await _database.SaveChangesAsync();
        TempData["Message"] = "Produk berhasil diperbarui.";
        return RedirectToPage("/Index");
    }

    private bool ValidateImage()
    {
        if (ImageFile is null)
        {
            return true;
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
        {
            ModelState.AddModelError(nameof(ImageFile), "Gunakan gambar JPG, PNG, atau WebP.");
        }
        else if (ImageFile.Length > 2 * 1024 * 1024)
        {
            ModelState.AddModelError(nameof(ImageFile), "Ukuran gambar maksimal 2 MB.");
        }

        return ModelState.IsValid;
    }

    private async Task<string> SaveImageAsync(IFormFile imageFile)
    {
        var uploadDirectory = Path.Combine(_environment.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadDirectory);
        var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(imageFile.FileName).ToLowerInvariant()}";
        var filePath = Path.Combine(uploadDirectory, fileName);
        await using var stream = System.IO.File.Create(filePath);
        await imageFile.CopyToAsync(stream);
        return $"/uploads/{fileName}";
    }

    private void DeleteImage(string? imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
        {
            return;
        }

        var filePath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
    }
}
