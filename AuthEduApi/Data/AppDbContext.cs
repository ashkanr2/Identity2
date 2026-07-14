using AuthEduApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthEduApi.Data;
 
public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext()
    {
        
    }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Products> Products { get; set; }

}
