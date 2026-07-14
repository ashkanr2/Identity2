// ═══════════════════════════════════════════════════════════════════
//  Program.cs  —  نقطه شروع برنامه
//  هر چیزی که اینجا تعریف میشه، قبل از اجرای اپلیکیشن آماده میشه
// ═══════════════════════════════════════════════════════════════════

using System.Text;
using AuthEduApi.Data;
using AuthEduApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
 

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default")
        ?? "Data Source=auth_edu.db"));

 

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    
    opt.Password.RequireDigit           = false;   
    opt.Password.RequiredLength         = 6;       
    opt.Password.RequireNonAlphanumeric = false;   
    opt.Password.RequireUppercase       = false;   

   
    opt.User.RequireUniqueEmail = true; 
})
    
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


 
var app = builder.Build();


 
if (app.Environment.IsDevelopment())
{
     
    app.UseSwagger();      
    app.UseSwaggerUI(c =>  
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Edu API v1"));
}

app.UseHttpsRedirection();  

 
// app.UseAuthentication();   
app.UseAuthorization();

app.MapControllers();  

app.Run();  