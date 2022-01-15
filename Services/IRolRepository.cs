using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenApi.Models;

namespace TokenApi.Services
{
    public interface IRolRepository
    {
        public List<String> GetRolXIdUser(int id);

        public List<TblRol> GetListRolXIdUser(int id);

        public IEnumerable GetAllRols();

        public Models.TblRol FindRol(int id);

        public bool CreateRol(string rol);

        public bool UpgradeRol(TblRol rol, string nuevoRol);

        public bool DeleteRol(TblRol rol);
    }
}
