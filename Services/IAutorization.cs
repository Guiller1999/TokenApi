using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenApi.Models;

namespace TokenApi.Services
{
    public interface IAutorization
    {
        public String GenerateToken(TblUsuario usuario, byte[] key);
    }
}
