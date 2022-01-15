using System;
using System.Collections.Generic;

#nullable disable

namespace TokenApi.Models
{
    public partial class TblRol
    {
        public TblRol()
        {
            TblRolUsuarios = new HashSet<TblRolUsuario>();
        }

        public int Id { get; set; }
        public string Rol { get; set; }

        public virtual ICollection<TblRolUsuario> TblRolUsuarios { get; set; }
    }
}
