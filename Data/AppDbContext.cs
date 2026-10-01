using Belajardotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace Belajardotnet.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();
}
