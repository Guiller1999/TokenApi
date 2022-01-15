using System;
using System.Collections.Generic;

#nullable disable

namespace TokenApi.Models
{
    public partial class TblPersona
    {
        public TblPersona()
        {
            TblUsuarios = new HashSet<TblUsuario>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Cedula { get; set; }

        public virtual ICollection<TblUsuario> TblUsuarios { get; set; }
    }
}
