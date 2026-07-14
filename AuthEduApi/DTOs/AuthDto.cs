using AuthEduApi.Models;
using System.ComponentModel.DataAnnotations;

namespace AuthEduApi.DTOs
{
     
    public record RegisterDto(
        [Required]
        string UserName,
         [Required]
        string Password,
        [Required]
        string Email,
         [Required]
        string FullName
    );

    public record LoginDto(
        [Required]
        string UserNameOrEmail,
         [Required]
        string Password
        
    );


    public record AuthResponseDto(
        bool IsSucces,
        string? Token,
        string Message,
        AppUser? User
        );
}
