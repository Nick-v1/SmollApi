using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Models.Dtos
{
    public record BanDto(string reason);
    public record UnbanDto(int UserId, string reason);
    public record BanDtoResult(int id, string action, string reason, DateTime BannedDate, int UserId);
}
