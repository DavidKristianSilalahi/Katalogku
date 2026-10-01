# KatalogKu

Aplikasi katalog dan toko online sederhana berbasis ASP.NET Core Razor Pages, .NET 9, Entity Framework Core, dan SQLite.

## Fitur

- Login, logout, dan register pelanggan
- Role Admin dan User
- Katalog produk dengan upload gambar
- Pesan sekarang atau simpan ke keranjang
- Keranjang dengan update jumlah dan hapus item
- Checkout dengan alamat dan metode pembayaran
- Dashboard admin untuk tambah, edit, dan hapus produk
- SQLite database otomatis

## Akun Demo

| Role | Username | Password |
|---|---|---|
| Admin | `admin` | `admin123` |
| User | `user` | `user123` |

## Menjalankan Project

Pastikan .NET SDK 9 sudah terpasang, lalu jalankan:

```powershell
dotnet restore
dotnet run
```

Buka URL yang muncul di terminal.

## Struktur Halaman

- `/Login` dan `/Register` untuk autentikasi
- `/Shop` untuk katalog pelanggan
- `/Cart` untuk keranjang
- `/Checkout` untuk checkout
- `/Admin` untuk dashboard admin

> Akun demo dibuat otomatis saat aplikasi pertama kali dijalankan. Untuk aplikasi production, password sebaiknya disimpan menggunakan hashing dan konfigurasi rahasia.
