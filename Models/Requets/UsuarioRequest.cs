using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenApi.Models
{
    public class UsuarioRequest
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String Cedula { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public int IDRol { get; set; }
    }
}
