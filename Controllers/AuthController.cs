using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TokenApi.Models;
using TokenApi.Services;

namespace TokenApi.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IAutorization _autorization;

        public AuthController(IConfiguration configuration, 
            IUserRepository userRepository, IAutorization autorization)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _autorization = autorization;
        }

        [AllowAnonymous]
        [HttpPost("iniciar")]       
        public IActionResult Login([FromBody] UserLogin user)
        {
            var secretKey = _configuration.GetValue<string>("Key");
            var key = Encoding.ASCII.GetBytes(secretKey);

            var usuario = _userRepository.GetUser(user);

            if (usuario == null) return BadRequest("El usuario no existe. Verifique sus credenciales de sesión");

            var token = _autorization.GenerateToken(usuario, key);

            return Ok(token);

        }
    }
}
