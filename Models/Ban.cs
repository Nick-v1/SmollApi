using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Models
{
    public class Ban
    {
        int UserID { get; set; }
        DateTime BannedDate { get; set; }
        string reason { get; set; }
    }
}
