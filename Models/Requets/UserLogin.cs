using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenApi.Models
{
    public class UserLogin
    {
        public int Id { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
    }
}
