using System.ComponentModel.DataAnnotations;

namespace SmollApi.Models.Dtos
{
    /// User record Dtos ---------------------------------------------------------------------------------
    public record UserSearchDto(int Id, string AccounType, byte Verified);
    public record UserAccountManagement([Required]string Username, [Required][EmailAddress]string Email, [Required]string Password);
    public record UserLoginDto([Required][EmailAddress]string Email, [Required]string Password);
    public record UserDtoDetails(int Id, string Username, string Email, string Password, string AccounType, byte Verified);
}
