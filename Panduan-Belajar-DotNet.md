# Panduan Belajar .NET Dari Nol

**Project latihan:** KatalogKu  
**Teknologi:** .NET 9, C#, ASP.NET Core Razor Pages, Entity Framework Core, SQLite

## Cara Menggunakan Panduan

Baca satu bab, buka file yang disebutkan di project, lalu jalankan aplikasinya. Cara terbaik belajar adalah mengikuti satu fitur dari form sampai database.

## 1. Apa Itu .NET?

.NET adalah platform dari Microsoft untuk membuat website, API, aplikasi desktop, cloud, dan mobile.

`.NET` bukan bahasa pemrograman. Bahasa yang paling umum digunakan adalah C#.

| Istilah | Arti |
|---|---|
| C# | Bahasa pemrograman |
| .NET | Platform aplikasi |
| ASP.NET Core | Framework untuk membuat web |
| Entity Framework Core | Alat penghubung C# dengan database |
| SQLite | Database berbentuk file |

## 2. Frontend dan Backend

**Frontend** adalah bagian yang dilihat user: HTML, CSS, JavaScript, layout, warna, form, tombol, dan gambar.

Pada KatalogKu:

```text
Pages/*.cshtml
wwwroot/css/site.css
wwwroot/js/site.js
```

**Backend** adalah bagian server: login, validasi, aturan bisnis, database, upload gambar, keranjang, dan checkout.

Pada KatalogKu:

```text
Pages/*.cshtml.cs
Models/
Data/
Services/
Program.cs
```

## 3. Alur Request Web

Saat user membuka `/Shop`:

```text
Browser
  -> GET /Shop
ASP.NET Core
  -> Shop.cshtml.cs
Shop PageModel
  -> AppDbContext
SQLite
  -> data produk
Razor
  -> HTML
Browser
  -> katalog produk
```

Saat user mengirim form:

```text
Form -> PageModel -> Service/DbContext -> Database -> Redirect -> Tampilan
```

## 4. Struktur Project KatalogKu

### `Program.cs`

Titik awal aplikasi. Mendaftarkan database, authentication, session, Razor Pages, dan middleware.

```csharp
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddAuthentication();
```

### `Models/Product.cs`

Bentuk data produk: `Id`, `Name`, `Category`, `Price`, `Stock`, dan `ImagePath`.

### `Models/User.cs`

Bentuk data akun: `Username`, `Password`, dan `Role`.

### `Data/AppDbContext.cs`

Jembatan antara C# dan SQLite.

```csharp
public DbSet<Product> Products => Set<Product>();
```

### `Pages/Shop.cshtml`

Tampilan katalog pelanggan.

### `Pages/Shop.cshtml.cs`

Logika katalog: mengambil produk, pesan sekarang, dan tambah ke keranjang.

### `Services/CartService.cs`

Logika keranjang: tambah, hapus, update jumlah, dan membaca session.

### `wwwroot/`

File statis seperti CSS, JavaScript, dan gambar upload.

## 5. CRUD

CRUD adalah empat operasi inti aplikasi data:

- **Create:** menambah produk.
- **Read:** melihat produk.
- **Update:** mengedit produk.
- **Delete:** menghapus produk.

Contoh EF Core:

```csharp
database.Products.Add(product);
database.Products.FindAsync(id);
database.Products.Remove(product);
await database.SaveChangesAsync();
```

## 6. GET, POST, dan Form

GET digunakan untuk menampilkan data:

```csharp
public async Task OnGetAsync()
{
    Products = await database.Products.ToListAsync();
}
```

POST digunakan untuk menambah atau mengubah data:

```csharp
public async Task<IActionResult> OnPostAsync()
{
    database.Products.Add(Product);
    await database.SaveChangesAsync();
    return RedirectToPage();
}
```

## 7. Database dan EF Core

SQLite cocok untuk belajar karena database disimpan dalam file. Entity Framework Core memungkinkan kita menggunakan C# untuk mengakses database.

```csharp
var products = await database.Products
    .OrderByDescending(p => p.CreatedAt)
    .ToListAsync();
```

Biasakan memakai `async` dan `await` untuk operasi database.

## 8. Login dan Role

**Authentication** menjawab: siapa user ini?  
**Authorization** menjawab: apa yang boleh dilakukan user ini?

KatalogKu memiliki dua role:

- `Admin`: mengelola produk.
- `User`: melihat katalog, keranjang, dan checkout.

```csharp
[Authorize(Roles = "Admin")]
[Authorize(Roles = "User")]
```

Akun demo:

```text
Admin: admin / admin123
User : user  / user123
```

Untuk production, password harus disimpan menggunakan hashing.

## 9. Keranjang dan Checkout

Pelanggan dapat memilih **Pesan sekarang** atau menyimpan produk ke keranjang.

```text
Shop -> pilih produk -> Cart -> update quantity -> Checkout -> selesai
```

Aplikasi production sebaiknya memiliki tabel `Order` dan `OrderItem`, mengurangi stok, dan status pesanan seperti `Pending`, `Diproses`, `Dikirim`, dan `Selesai`.

## 10. Cara Belajar dari KatalogKu

1. Pilih fitur tambah produk.
2. Baca form di `Index.cshtml`.
3. Ikuti handler `OnPostAsync` di `Index.cshtml.cs`.
4. Lihat model `Product`.
5. Lihat `AppDbContext`.
6. Jalankan aplikasi.
7. Ubah satu bagian kecil.
8. Jalankan `dotnet build` dan tes lagi.

Ulangi metode ini untuk login, upload gambar, keranjang, dan checkout.

## 11. Roadmap Belajar .NET

### Tahap 1: C#

Variable, tipe data, `if`, loop, method, class, object, List, LINQ, exception, async, dan await.

### Tahap 2: Web

HTML, CSS, HTTP, URL, GET, POST, form, validation, cookie, dan session.

### Tahap 3: ASP.NET Core

Razor Pages, MVC, routing, model binding, dependency injection, middleware, authentication, dan authorization.

### Tahap 4: Database

SQL dasar, primary key, foreign key, relasi tabel, EF Core, dan migration.

### Tahap 5: Lanjutan

REST API, JSON, JWT, testing, logging, Docker, dan deployment.

## 12. Latihan Berikutnya

1. Tambahkan pencarian produk.
2. Tambahkan filter kategori.
3. Kurangi stok setelah checkout.
4. Buat tabel `Order` dan `OrderItem`.
5. Buat halaman riwayat pesanan user.
6. Buat admin mengubah status pesanan.
7. Tambahkan API produk.
8. Ganti password biasa dengan hashing.
9. Deploy aplikasi ke hosting.

## Kesimpulan

Target utama belajar .NET adalah memahami alur berikut:

```text
Form -> PageModel -> Service/DbContext -> Database -> Response -> Tampilan
```

Jika alur ini sudah dipahami, Anda sudah memiliki fondasi kuat untuk menjadi developer ASP.NET Core.

## Ekspor Menjadi PDF

Buka file ini di VS Code, tekan `Ctrl + Shift + V` untuk melihat preview Markdown, lalu pilih **Print** dari browser dan pilih **Save as PDF**.
