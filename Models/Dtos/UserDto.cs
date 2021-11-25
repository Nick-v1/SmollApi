using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Models.Dtos
{
    /// User record Dtos ---------------------------------------------------------------------------------
    public record UserSearchDto(int Id, string AccounType, byte Verified);
    public record UserAccountManagement(string Username, string Email, string Password);
    public record UserLoginDto(string Username, string password);
    public record UserDtoDetails(int Id, string Username, string Email, string Password, string AccounType, byte Verified);
}
