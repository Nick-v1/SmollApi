using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Models
{
    public class Favourite
    {
        int UserId { get; set; }
        int PhonesId { get; set; }
    }
}