using System.ComponentModel.DataAnnotations;

namespace Belajardotnet.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nama produk wajib diisi.")]
    [StringLength(100)]
    [Display(Name = "Nama produk")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kategori wajib diisi.")]
    [StringLength(60)]
    public string Category { get; set; } = string.Empty;

    [Range(0.01, 999999999, ErrorMessage = "Harga harus lebih dari 0.")]
    [Display(Name = "Harga")]
    public decimal Price { get; set; }

    [Range(0, 999999, ErrorMessage = "Stok tidak boleh negatif.")]
    public int Stock { get; set; }

    public string? ImagePath { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
