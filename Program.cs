using Belajardotnet.Data;
using Belajardotnet.Models;
using Belajardotnet.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CartService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Login";
    });
builder.Services.AddAuthorization();
builder.Services.AddSession(options => options.IdleTimeout = TimeSpan.FromHours(8));
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    database.Database.EnsureCreated();
    var connection = database.Database.GetDbConnection();
    connection.Open();
    using var command = connection.CreateCommand();
    command.CommandText = "SELECT COUNT(*) FROM pragma_table_info('Products') WHERE name = 'ImagePath'";
    var imageColumnExists = Convert.ToInt32(command.ExecuteScalar()) > 0;
    if (!imageColumnExists)
    {
        database.Database.ExecuteSqlRaw("ALTER TABLE Products ADD COLUMN ImagePath TEXT NULL");
    }
    database.Database.ExecuteSqlRaw("CREATE TABLE IF NOT EXISTS Users (Id INTEGER NOT NULL CONSTRAINT PK_Users PRIMARY KEY AUTOINCREMENT, Username TEXT NOT NULL UNIQUE, Password TEXT NOT NULL, Role TEXT NOT NULL)");
    if (!database.Users.Any())
    {
        database.Users.AddRange(new User { Username = "admin", Password = "admin123", Role = "Admin" }, new User { Username = "user", Password = "user123", Role = "User" });
        database.SaveChanges();
    }
}

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
