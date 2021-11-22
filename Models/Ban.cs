using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Models
{
    public class Ban
    {
        [Key] public int UserID { get; set; }
        public DateTime BannedDate { get; private set; }
        public string reason { get; set; }

        public void setDate(DateTime date)
        {
            BannedDate = date;
        }
    }
}
