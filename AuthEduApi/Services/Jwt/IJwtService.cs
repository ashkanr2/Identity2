using AuthEduApi.Models;

namespace AuthEduApi.Services.Jwt
{
    public interface IJwtService
    {
        string GenerateToken( AppUser appUser , IList<string>Roles);
    }
}
