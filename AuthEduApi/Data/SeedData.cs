using AuthEduApi.Constants;
using AuthEduApi.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthEduApi.Data;

public static class SeedData
{
    /// <summary>
    /// هر بار که اپ Start میشه این متد اجرا میشه
    /// Roles و ادمین پیش‌فرض رو می‌سازه (اگه نباشن)
    /// </summary>
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var db          = services.GetRequiredService<AppDbContext>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();

        // دیتابیس رو می‌سازه اگه وجود نداشته باشه
        await db.Database.EnsureCreatedAsync();

        // ─── ساخت Roles ──────────────────────────────────────
        string[] roles = [AppRoles.Admin, AppRoles.User];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // ─── ساخت ادمین پیش‌فرض ──────────────────────────────
        const string adminEmail    = "admin@edu.com";
        const string adminPassword = "admin123";

        if (await userManager.FindByEmailAsync(adminEmail) is null)
        {
            var admin = new AppUser
            {
                UserName       = "admin",
                Email          = adminEmail,
                FullName       = "Super Admin",
                IsApproved     = true,   // ادمین نیازی به تایید نداره
                IsPro          = true,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, AppRoles.Admin);
        }

        var existProduct = db.Products.Any();

        if (!existProduct)
        {
           var productList = new List<Products>
                 {
                     new Products { Name = "Product 1", Description = "Description for Product 1", Price = 10.99m },
                     new Products { Name = "Product 2", Description = "Description for Product 2", Price = 19.99m },
                     new Products { Name = "Product 3", Description = "Description for Product 3", Price = 5.49m },
                     new Products { Name = "Product 4", Description = "Description for Product 4", Price = 15.00m },
                     new Products { Name = "Product 5", Description = "Description for Product 5", Price = 7.25m }
                 };
            db.Products.AddRange(productList);
            db.SaveChanges();
        }
        Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        Console.WriteLine("✅  Seed Data OK");
        Console.WriteLine($"👑  Admin → {adminEmail} / {adminPassword}");
        Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
    }
}
