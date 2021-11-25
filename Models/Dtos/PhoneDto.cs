using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Models.Dtos
{
    public record PhoneDto(string Name, string Manifacturer, int RAM, int ROM, double ScreenSize, string OS);
}
