using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenApi.Models;

namespace TokenApi.Services
{
    public class RolRepository : IRolRepository
    {
        private readonly Models.TokenContext _tokenContext;

        public RolRepository(Models.TokenContext tokenContext)
        {
            _tokenContext = tokenContext;
        }


        public IEnumerable GetAllRols()
        {
            var ltsRoles = _tokenContext.TblRols.Select(rol => rol).ToList();

            return ltsRoles;
        }

        public List<string> GetRolXIdUser(int id)
        {
            var ltsRoles = _tokenContext.TblRolUsuarios.Where(p => p.IdUsuario == id)
                                .Join(_tokenContext.TblRols, usuario => usuario.IdRol, rol => rol.Id, (usuario, rol) => rol.Rol)
                                .ToList();
            return ltsRoles;
        }

        public List<TblRol> GetListRolXIdUser(int id)
        {
            var ltsRoles = _tokenContext.TblRolUsuarios.Where(p => p.IdUsuario == id)
                                .Join(_tokenContext.TblRols, usuario => usuario.IdRol, rol => rol.Id, (usuario, rol) => new
                                {
                                    rol.Id,
                                    rol.Rol
                                })
                                .Select(p => new TblRol {
                                    Id = p.Id,
                                    Rol = p.Rol
                                })
                                .ToList();
            return ltsRoles;
        }


        public Models.TblRol FindRol(int id)
        {
            var rol = _tokenContext.TblRols.Find(id);

            return rol;
        }


        public bool CreateRol(string rol)
        {
            try
            {
                Models.TblRol nuevoRol = new Models.TblRol
                {
                    Rol = rol
                };
                _tokenContext.Add(nuevoRol);
                _tokenContext.SaveChanges();

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }


        public bool UpgradeRol(Models.TblRol rol, string nuevoRol)
        {
            try
            {
                rol.Id = rol.Id;
                rol.Rol = nuevoRol;
                _tokenContext.Entry(rol).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _tokenContext.SaveChanges();

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }


        public bool DeleteRol(Models.TblRol rol)
        {
            try
            {
                _tokenContext.TblRols.Remove(rol);
                _tokenContext.SaveChanges();

                return true;
            }
            catch(Exception)
            {
                return false;
            }

        }
    }
}
