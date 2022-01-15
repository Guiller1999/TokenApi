using System;
using System.Collections.Generic;

#nullable disable

namespace TokenApi.Models
{
    public partial class TblRolUsuario
    {
        public int Id { get; set; }
        public int IdRol { get; set; }
        public int IdUsuario { get; set; }

        public virtual TblRol IdRolNavigation { get; set; }
        public virtual TblUsuario IdUsuarioNavigation { get; set; }
    }
}
