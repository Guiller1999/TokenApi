using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenApi.Services;
using TokenApi.Models.Response;
using System.Collections;

namespace TokenApi.Controllers
{
    [Authorize(Roles = "administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IRolRepository _rolRepository;

        public RolController(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }


        [HttpGet("ConsultarRoles")]
        public IActionResult ConsultarRoles()
        {
            try
            {
                var response = _rolRepository.GetAllRols();
                Result<IEnumerable> respuesta = new Result<IEnumerable>
                {
                    Status = 200,
                    Message = response
                };

                return Ok(respuesta);
            }
            catch
            {
                Result<string> respuesta = new Result<string>
                {
                    Status = 200,
                    Message = "Ha ocurrido un error al momento de consultar los datos. Intente más tarde"
                };
                return BadRequest(respuesta);
            }
        }


        [HttpPost("CrearRol")]
        public IActionResult CrearRol([FromForm]string rol)
        {
            Result<string> respuesta = new Result<string>();
            var response = _rolRepository.CreateRol(rol);

            if(response)
            {
                respuesta.Status = StatusCodes.Status200OK;
                respuesta.Message = "Rol creado correctamente";
                return Ok(respuesta);
            }
            else
            {
                respuesta.Status = StatusCodes.Status400BadRequest;
                respuesta.Message = "Ha ocurrido un error al momento de crear el rol";
                return BadRequest(respuesta);
            }
        }


        [HttpPut("ActualizarRol/{idRol}")]
        public IActionResult ActualizarRol([FromRoute]int idRol, [FromForm]string nuevoRol) 
        {
            Result<string> respuesta = new Result<string>();

            var rol = _rolRepository.FindRol(idRol);

            if (rol == null)
            {
                respuesta.Status = StatusCodes.Status404NotFound;
                respuesta.Message = "Error. No existe el rol al que se hace mención";
                return NotFound(respuesta);
            }
            var response = _rolRepository.UpgradeRol(rol, nuevoRol);

            if(response)
            {
                respuesta.Status = StatusCodes.Status200OK;
                respuesta.Message = "Rol actualizado correctamente";
                return Ok(respuesta);
            }
            else
            {
                respuesta.Status = StatusCodes.Status400BadRequest;
                respuesta.Message = "Ha ocurrido un error al momento de actualizar el rol";
                return BadRequest(respuesta);
            }
        }

        [HttpDelete("BorrarRol/{idRol}")]
        public IActionResult BorrarRol([FromRoute]int idRol)
        {
            Result<string> respuesta = new Result<string>();
            var rol = _rolRepository.FindRol(idRol);

            if (rol == null)
            {
                respuesta.Status = StatusCodes.Status404NotFound;
                respuesta.Message = "Error. No existe el rol al que se hace mención";
                return NotFound(respuesta);
            }
            
            var response = _rolRepository.DeleteRol(rol);

            if (response)
            {
                respuesta.Status = StatusCodes.Status200OK;
                respuesta.Message = "Rol eliminado correctamente";
                return Ok(respuesta);
            }
            else
            {
                respuesta.Status = StatusCodes.Status400BadRequest;
                respuesta.Message = "Ha ocurrido un error al momento de eliminar el rol";
                return BadRequest(respuesta);
            }
        }
    }
}
