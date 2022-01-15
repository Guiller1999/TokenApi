using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenApi.Models
{
    public class UsuarioResponse
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Username { get; set; }

        public List<String> Roles { get; set; }
    }
}
