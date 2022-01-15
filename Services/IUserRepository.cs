using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenApi.Models;

namespace TokenApi.Services
{
    public interface IUserRepository
    {

        public TblUsuario GetUsuarioXId(int idUsuario);

        public TblPersona GetPersonXCedula(String cedula);

        public TblPersona GetPersonXId(int id);

        public TblUsuario GetUser(UserLogin user);

        public IEnumerable GetAllUsers();

        public TblRolUsuario GetUsuarioRol(int idUsuario, int idRol);

        public UsuarioResponse GetUsuarioRolXId(int id);

        public bool CreatePerson(UsuarioRequest nuevoUsuario);

        public bool CreateUser(UsuarioRequest nuevoUsuario);

        public bool AsignRol(int idUsuario, int idRol);

        public bool UpdateUser(UsuarioRequest usuario, int idPersona);

        public bool UpdatePerson(UsuarioRequest usuario, int idPersona);

        public bool DeleteRolUser(TblRolUsuario rolUsuario);

        public bool DeletelUser(TblUsuario usuario);

        public bool DeletelPerson(TblPersona persona);
    }
}
