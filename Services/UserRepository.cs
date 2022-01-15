using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenApi.Models;

namespace TokenApi.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly Models.TokenContext _tokenContext;
        private readonly IRolRepository _rolRepository;


        public UserRepository(Models.TokenContext tokenContext, IRolRepository rolRepository)
        {
            _tokenContext = tokenContext;
            _rolRepository = rolRepository;
        }


        public TblUsuario GetUsuarioXId(int idUsuario)
        {
            var user = _tokenContext.TblUsuarios.Where(p => p.Id == idUsuario).FirstOrDefault();

            return user;
        }

        public TblPersona GetPersonXCedula(String cedula)
        {
            var person = _tokenContext.TblPersonas.Where(p => p.Cedula == cedula).FirstOrDefault();

            return person;
        }


        public TblPersona GetPersonXId(int id)
        {
            var person = _tokenContext.TblPersonas.Where(p => p.Id == id).FirstOrDefault();
               
            return person;
        }


        public TblUsuario GetUser(UserLogin user)
        {
            var usuario = _tokenContext.TblUsuarios.Where(p => p.Username == user.Username && p.Password == user.Password);

            if (usuario == null) return null;

            return usuario.FirstOrDefault();
        }

        public IEnumerable GetAllUsers()
        {
            var ltsUsuario = _tokenContext.TblUsuarios
                .Join(_tokenContext.TblPersonas, usuario => usuario.IdPersona, persona => persona.Id, (usuario, persona) => new
                {
                    usuario,
                    persona
                })
                .Select(usuarios => new UsuarioResponse
                {
                    Id = usuarios.usuario.Id,
                    Name = usuarios.persona.Nombre + " " + usuarios.persona.Apellidos,
                    Username = usuarios.usuario.Username,
                    Roles = _rolRepository.GetRolXIdUser(usuarios.usuario.Id)
                }).ToList();

            return ltsUsuario;
        }


        public UsuarioResponse GetUsuarioRolXId(int id)
        {
            var usuarioInfo = _tokenContext.TblUsuarios.Where(p => p.Id == id)
                .Join(_tokenContext.TblPersonas, usuario => usuario.IdPersona, persona => persona.Id, (usuario, persona) => new
                {
                    usuario,
                    persona
                })
                .Select(usuarios => new UsuarioResponse
                {
                    Id = usuarios.usuario.Id,
                    Name = usuarios.persona.Nombre + " " + usuarios.persona.Apellidos,
                    Username = usuarios.usuario.Username,
                    Roles = _rolRepository.GetRolXIdUser(usuarios.usuario.Id)
                }).FirstOrDefault();

            return usuarioInfo;
        }

        public TblRolUsuario GetUsuarioRol(int idUsuario, int idRol)
        {
            var rolUser = _tokenContext.TblRolUsuarios.Where(p => p.IdUsuario == idUsuario && p.IdRol == idRol)
                            .FirstOrDefault();

            return rolUser;
        }

        public bool CreatePerson(UsuarioRequest nuevoUsuario)
        {
            try
            {
                Models.TblPersona persona = new Models.TblPersona
                {
                    Nombre = nuevoUsuario.Name,
                    Apellidos = nuevoUsuario.LastName,
                    Cedula = nuevoUsuario.Cedula,
                    Email = nuevoUsuario.Email
                };

                _tokenContext.Add(persona);
                _tokenContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool CreateUser(UsuarioRequest nuevoUsuario)
        {
            try
            {
                Models.TblUsuario usuario = new Models.TblUsuario
                {
                    Username = nuevoUsuario.Username,
                    Password = nuevoUsuario.Password,
                    IdPersona = nuevoUsuario.ID
                };

                _tokenContext.Add(usuario);
                _tokenContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AsignRol(int idUsuario, int idRol)
        {
            try
            {
                Models.TblRolUsuario usuario = new Models.TblRolUsuario
                {
                    IdRol = idRol,
                    IdUsuario = idUsuario
                };

                _tokenContext.Add(usuario);
                _tokenContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateUser(UsuarioRequest usuario, int idPersona)
        {
            try
            {
                TblUsuario tblUsuario = GetUser(new UserLogin() { Username = usuario.Username, Password = usuario.Password});
                tblUsuario.Id = usuario.ID;
                tblUsuario.Username = usuario.Username;
                tblUsuario.Password = usuario.Password;
                tblUsuario.IdPersona = idPersona;

                _tokenContext.Entry(tblUsuario).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _tokenContext.SaveChanges();

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool UpdatePerson(UsuarioRequest usuario, int idPersona)
        {
            try
            {
                TblPersona tblPersona = GetPersonXCedula(usuario.Cedula);
                tblPersona.Id = idPersona;
                tblPersona.Nombre = usuario.Name;
                tblPersona.Apellidos = usuario.LastName;
                tblPersona.Email = usuario.Email;
                tblPersona.Cedula = usuario.Cedula;

                _tokenContext.Entry(tblPersona).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _tokenContext.SaveChanges();

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool DeleteRolUser(TblRolUsuario rolUsuario)
        {
            try
            {
                _tokenContext.TblRolUsuarios.Remove(rolUsuario);
                _tokenContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeletelUser(TblUsuario usuario)
        {
            try
            {
                _tokenContext.TblUsuarios.Remove(usuario);
                _tokenContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeletelPerson(TblPersona persona)
        {
            try
            {
                _tokenContext.TblPersonas.Remove(persona);
                _tokenContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
