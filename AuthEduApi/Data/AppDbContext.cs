using AuthEduApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthEduApi.Data;

/// <summary>
/// DbContext ما از IdentityDbContext ارث‌بری می‌کنه
///
/// 💡 IdentityDbContext چیه؟
///   یه DbContext آماده‌ست که جدول‌های Identity رو برات می‌سازه:
///   AspNetUsers, AspNetRoles, AspNetUserRoles,
///   AspNetUserClaims, AspNetUserTokens و...
///
///   ما فقط باید AppUser رو بهش بدیم تا بدونه
///   جدول Users چه شکلیه
/// </summary>
public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
