
using System.Text;
using AuthEduApi.Constants;
using AuthEduApi.Data;
using AuthEduApi.Models;
using AuthEduApi.Services;
using AuthEduApi.Services.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddDbContext<AppDbContext>(opt =>
//    opt.UseSqlite(builder.Configuration.GetConnectionString("Default")
//        ?? "Data Source=auth_edu.db"));

builder.Services.AddDbContext<AppDbContext>(options => options
.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    
    opt.Password.RequireDigit           = false;   
    opt.Password.RequiredLength         = 4;       
    opt.Password.RequireNonAlphanumeric = false;   
    opt.Password.RequireUppercase       = false;   
    opt.User.RequireUniqueEmail = true; 
})
    
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var key = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
        };
    });


builder.Services.AddAuthorization
    (opt =>
    {

        opt.AddPolicy(AppPolicies.AdminOnly, policy => policy.RequireRole(AppRoles.Admin));
        opt.AddPolicy(AppPolicies.ApprovedUser, policy => policy.RequireAuthenticatedUser());
        opt.AddPolicy(AppPolicies.ProUser, policy => policy.RequireClaim(AppClaims.IsPro,"True"));
    });





builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "🔐 Auth Edu API ",
        Version = "v1",
    });

    // اضافه کردن Bearer Token به Swagger
    var bearerScheme = new OpenApiSecurityScheme
    {
        Description = "Good Bye Gp 4",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    };
    c.AddSecurityDefinition("Bearer", bearerScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});




var app = builder.Build();

using (var scope = app.Services.CreateScope())
    await SeedData.InitializeAsync(scope.ServiceProvider);
 
if (app.Environment.IsDevelopment())
{
     
    app.UseSwagger();      
    app.UseSwaggerUI(c =>  
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Edu API v1"));
}

app.UseHttpsRedirection();  

 
app.UseAuthentication();   
app.UseAuthorization();

app.MapControllers();  

app.Run();  