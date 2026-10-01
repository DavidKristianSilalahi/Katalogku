using Belajardotnet.Data;
using Belajardotnet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Belajardotnet.Pages;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly AppDbContext _database;
    private readonly IWebHostEnvironment _environment;

    public IndexModel(AppDbContext database, IWebHostEnvironment environment)
    {
        _database = database;
        _environment = environment;
    }

    public IList<Product> Products { get; private set; } = [];

    [BindProperty]
    public Product Product { get; set; } = new();

    [BindProperty]
    public IFormFile? ImageFile { get; set; }

    public async Task OnGetAsync()
    {
        Products = await _database.Products
            .OrderByDescending(product => product.CreatedAt)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ValidateImage())
        {
            await OnGetAsync();
            return Page();
        }

        if (!ModelState.IsValid)
        {
            await OnGetAsync();
            return Page();
        }

        Product.ImagePath = await SaveImageAsync(ImageFile);
        _database.Products.Add(Product);
        await _database.SaveChangesAsync();
        TempData["Message"] = "Produk berhasil ditambahkan.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var product = await _database.Products.FindAsync(id);
        if (product is not null)
        {
            DeleteImage(product.ImagePath);
            _database.Products.Remove(product);
            await _database.SaveChangesAsync();
            TempData["Message"] = "Produk berhasil dihapus.";
        }

        return RedirectToPage();

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

    private async Task<string?> SaveImageAsync(IFormFile? imageFile)
    {
        if (imageFile is null)
        {
            return null;
        }

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
