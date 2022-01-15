using System;
using System.Collections.Generic;

#nullable disable

namespace TokenApi.Models
{
    public partial class TblUsuario
    {
        public TblUsuario()
        {
            TblRolUsuarios = new HashSet<TblRolUsuario>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int IdPersona { get; set; }

        public virtual TblPersona IdPersonaNavigation { get; set; }
        public virtual ICollection<TblRolUsuario> TblRolUsuarios { get; set; }
    }
}
