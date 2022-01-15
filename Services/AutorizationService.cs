
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

namespace TokenApi.Services
{
    public class AutorizationService : IAutorization
    {

        private readonly IUserRepository _userRepository;

        public AutorizationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string GenerateToken(TblUsuario usuario, byte[] key)
        {
            UsuarioResponse usuarioResponse = (UsuarioResponse)_userRepository.GetUsuarioRolXId(usuario.Id);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(usuarioResponse),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(createdToken);
        }


        private static ClaimsIdentity GenerateClaims(UsuarioResponse usuario)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim(ClaimTypes.GivenName, usuario.Name)
            };

            foreach (String rol in usuario.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            return new ClaimsIdentity(claims.ToArray());
        }
    }
}
