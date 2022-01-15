using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TokenApi.Models;
using TokenApi.Services;
using TokenApi.Models.Response;

namespace TokenApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IRolRepository _rolRepository;
        private readonly IUserRepository _userRepository;

        public ValuesController(IUserRepository userRepository, IRolRepository rolRepository)
        {
            _userRepository = userRepository;
            _rolRepository = rolRepository;
        }


        [Authorize(Roles = "administrador, usuario")]
        [HttpGet("ConsultarDatos")]
        public IActionResult ConsultarDatos()
        {
            List<String> roles = new List<string>();

            foreach(var rol in User.FindAll(ClaimTypes.Role))
            {
                roles.Add(rol.Value);
            }

            UsuarioResponse usuario = new UsuarioResponse
            {
                Id = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                Username = User.FindFirst(ClaimTypes.Name).Value,
                Name = User.FindFirst(ClaimTypes.GivenName).Value,
                Roles = roles
            };

            Result<UsuarioResponse> respuesta = new Result<UsuarioResponse>
            {
                Status = 200,
                Message = usuario
            };

            return Ok(respuesta);
        
        }

        [Authorize(Roles = "administrador")]
        [HttpGet("ConsultarUsuarios")]
        public IActionResult ConsultarUsuarios()
        {
            List<UsuarioResponse> usuarios = (List<UsuarioResponse>)_userRepository.GetAllUsers();
            Result<List<UsuarioResponse>> respuesta = new Result<List<UsuarioResponse>>
            {
                Status = 200,
                Message = usuarios
            };

            return Ok(respuesta);
        }


        [Authorize(Roles = "administrador")]
        [HttpPost("CrearUsuario")]
        public IActionResult CrearUsuario(UsuarioRequest nuevoUsuario)
        {
            Result<string> respuesta = new Result<string>();

            // Valido que el rol exista
            var rol = _rolRepository.FindRol(nuevoUsuario.IDRol);
            if (rol == null)
            {
                respuesta.Status = StatusCodes.Status404NotFound;
                respuesta.Message = "No existe un rol con ID: " + nuevoUsuario.IDRol;
                return BadRequest(respuesta);
            }
            // Valido que no exista persona con la misma cedula
            var person = _userRepository.GetPersonXCedula(nuevoUsuario.Cedula);
            if (person != null)
            {
                respuesta.Status = StatusCodes.Status400BadRequest;
                respuesta.Message = "Ya existe un registro creado con la cedula " + nuevoUsuario.Cedula;
                return BadRequest(respuesta);
            }
            // Se crea el registro para la nueva persona
            var responsePerson = _userRepository.CreatePerson(nuevoUsuario);
            if (responsePerson == false)
            {
                respuesta.Status = StatusCodes.Status400BadRequest;
                respuesta.Message = "Ha ocurrido un error al momento de crear el registro de la persona";
                return BadRequest(respuesta);
            }
            // Se crea el registro de usuario
            var personSearch = _userRepository.GetPersonXCedula(nuevoUsuario.Cedula);
            nuevoUsuario.ID = personSearch.Id;
            var responseUser = _userRepository.CreateUser(nuevoUsuario);

            if (responseUser == false)
            {
                respuesta.Status = StatusCodes.Status400BadRequest;
                respuesta.Message = "Ha ocurrido un error al momento de crear el registro del usuario";
                return BadRequest(respuesta);
            }
            // Se asigna el rol al usuario
            UserLogin user = new UserLogin
            {
                Username = nuevoUsuario.Username,
                Password = nuevoUsuario.Password
            };

            var idUser = _userRepository.GetUser(user).Id;
            var response = _userRepository.AsignRol(idUser, nuevoUsuario.IDRol);

            if(response)
            {
                respuesta.Status = StatusCodes.Status200OK;
                respuesta.Message = "Nuevo usuario creado correctamente";
                return Ok(respuesta);
            }
            else
            {
                respuesta.Status = StatusCodes.Status400BadRequest;
                respuesta.Message = "Error. No se pudo asignar el rol al usuario";
                return BadRequest(respuesta);
            }
        }


        [Authorize(Roles = "administrador")]
        [HttpPost("ActualizarUsuario")]
        public IActionResult ActualizarUsuario(UsuarioRequest nuevoUsuario)
        {
            Result<string> respuesta = new Result<string>();
            // Valido que el rol exista
            var rol = _rolRepository.FindRol(nuevoUsuario.IDRol);
            if (rol == null)
            {
                respuesta.Status = StatusCodes.Status404NotFound;
                respuesta.Message = "No existe un rol con ID: " + nuevoUsuario.IDRol;
                return BadRequest(respuesta);
            }    
            // Se crea el registro para la nueva persona
            var persona = _userRepository.GetPersonXCedula(nuevoUsuario.Cedula);
            var responsePerson = _userRepository.UpdatePerson(nuevoUsuario, persona.Id);
            if (responsePerson == false)
            {
                respuesta.Status = StatusCodes.Status400BadRequest;
                respuesta.Message = "Ha ocurrido un error al momento de actualizar el registro de la persona";
                return BadRequest(respuesta);
            }
            // Se crea el registro de usuario
            var usuarioSearch = _userRepository.GetUser(new UserLogin() 
                { Username = nuevoUsuario.Username, Password = nuevoUsuario.Password });
            nuevoUsuario.ID = usuarioSearch.Id;
            var response = _userRepository.UpdateUser(nuevoUsuario, persona.Id);

            if(response)
            {
                respuesta.Status = StatusCodes.Status200OK;
                respuesta.Message = "Usuario actualizado correctamente";
                return Ok(respuesta);
            }
            else
            {
                respuesta.Status = StatusCodes.Status400BadRequest;
                respuesta.Message = "Error. No se pudo actualizar los datos del usuario";
                return BadRequest(respuesta);
            }
        }


        [Authorize(Roles = "administrador")]
        [HttpPost("AgregarRolUsuario")]
        public IActionResult AgregarUsuarioRol(UsuarioRequest nuevoUsuario)
        {
            Result<string> respuesta = new Result<string>();
            var usuario = _userRepository.GetUsuarioXId(nuevoUsuario.ID);

            if (usuario == null)
            {
                respuesta.Status = StatusCodes.Status404NotFound;
                respuesta.Message = "No existe el usuario seleccionado";
                return NotFound(respuesta);
            }
            var rol = _rolRepository.FindRol(nuevoUsuario.IDRol);

            if (rol == null)
            {
                respuesta.Status = StatusCodes.Status404NotFound;
                respuesta.Message = "No existe el rol seleccionado";
                return NotFound(respuesta);
            }

            var rolUser = _userRepository.GetUsuarioRol(nuevoUsuario.ID, nuevoUsuario.IDRol);

            if (rolUser != null)
            {
                respuesta.Status = StatusCodes.Status400BadRequest;
                respuesta.Message = "El usuario ya tiene asignado el rol seleccionado";
                return BadRequest(respuesta);
            }

            var response = _userRepository.AsignRol(nuevoUsuario.ID, nuevoUsuario.IDRol);

            if(response)
            {
                respuesta.Status = StatusCodes.Status200OK;
                respuesta.Message = "Rol asignado correctamente";
                return Ok(respuesta);
            }
            else
            {
                respuesta.Status = StatusCodes.Status400BadRequest;
                respuesta.Message = "Error al momento de asignar rol";
                return BadRequest(respuesta);
            }
        }


        [Authorize(Roles = "administrador")]
        [HttpPost("EliminarRolUsuario")]
        public IActionResult EliminarRolUsuario(UsuarioRequest nuevoUsuario)
        {
            Result<string> respuesta = new Result<string>();
            var usuario = _userRepository.GetUsuarioXId(nuevoUsuario.ID);

            if (usuario == null)
            {
                respuesta.Status = StatusCodes.Status404NotFound;
                respuesta.Message = "No existe el usuario seleccionado";
                return BadRequest(respuesta);
            }

            var rol = _rolRepository.FindRol(nuevoUsuario.IDRol);

            if (rol == null)
            {
                respuesta.Status = StatusCodes.Status404NotFound;
                respuesta.Message = "No existe el rol seleccionado";
                return NotFound(respuesta);
            }

            var rolUser = _userRepository.GetUsuarioRol(nuevoUsuario.ID, nuevoUsuario.IDRol);

            if (rolUser == null)
            {
                respuesta.Status = StatusCodes.Status400BadRequest;
                respuesta.Message = "El usuario no tiene asignado el rol a eliminar";
                return BadRequest(respuesta);
            }

            var response = _userRepository.DeleteRolUser(rolUser);

            if(response)
            {
                respuesta.Status = StatusCodes.Status200OK;
                respuesta.Message = "Se ha quitado correctamente el rol al usuario seleccionado";
                return Ok(respuesta);
            }
            else
            {
                respuesta.Status = StatusCodes.Status400BadRequest;
                respuesta.Message = "Ha ocurrido un error al momento de quitar el rol";
                return BadRequest(respuesta);
            }
        }

        [Authorize(Roles = "administrador")]
        [HttpDelete("EliminarUsuario/{idUsuario}")]
        public IActionResult EliminarUsuario(int idUsuario)
        {
            Result<string> respuesta = new Result<string>();
            var usuario = _userRepository.GetUsuarioXId(idUsuario);

            if (usuario == null)
            {
                respuesta.Status = StatusCodes.Status404NotFound;
                respuesta.Message = "Error. No existe el usuario seleccionado";
                return NotFound(respuesta);
            }

            var roles = _rolRepository.GetListRolXIdUser(usuario.Id);

            var numRoles = roles.Count;

            if(numRoles > 0)
            {
                var responseCount = EliminarRolesUsuario(usuario, roles);

                if (responseCount != numRoles)
                {
                    respuesta.Status = StatusCodes.Status400BadRequest;
                    respuesta.Message = "Error. Hubo roles del usuario que no se eliminaron. Intente de nuevo";
                    return BadRequest(respuesta);
                }
            }

            var responseUser = _userRepository.DeletelUser(usuario);

            if (responseUser == false)
            {
                respuesta.Status = StatusCodes.Status400BadRequest;
                respuesta.Message = "Error al momento de eliminar el registro de usuario. Intente de nuevo";
                return BadRequest(respuesta);
            }
            
            var person = _userRepository.GetPersonXId(usuario.IdPersona);

            var responsePerson = _userRepository.DeletelPerson(person);

            if(responsePerson)
            {
                respuesta.Status = StatusCodes.Status200OK;
                respuesta.Message = "Registro de persona eliminado correctamente";
                return Ok(respuesta);
            }
            else
            {
                respuesta.Status = StatusCodes.Status400BadRequest;
                respuesta.Message = "No se ha podido eliminar el registro de la persona";
                return BadRequest(respuesta);
            }
        }


        private int EliminarRolesUsuario(TblUsuario usuario, List<TblRol> roles)
        {
            int count = 0;

            foreach(TblRol rol in roles)
            {
                var rolUser = _userRepository.GetUsuarioRol(usuario.Id, rol.Id);
                _userRepository.DeleteRolUser(rolUser);
                count++;
            }

            return count;
        }
    }
}
