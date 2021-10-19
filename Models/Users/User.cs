using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Models
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string accounType { get; private set; }
        public bool verified { get; private set;}

        public void setType(string s)
        {
            accounType = s;
        }
        public void setVer(bool verify)
        {
            verified = verify;
        }
    }
}
