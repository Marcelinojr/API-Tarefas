using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SistemaTarefa.Models;

namespace SistemaTarefa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        [HttpPost]

        public IActionResult Login([FromBody] LoginModel loginModel)
        {

            var token = GerarTokenJwt();
            return Ok(new { Token = token });
        }


        private string GerarTokenJwt()
        {
            string chaveSecreta = "eb5ab58e-3146-4be0-bce7-e42a972cb6d7";

            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta));
            var credencials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("role","admin"),
                new Claim("username","Administrador")
            };

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: "SistemaTarefa",
                audience: "SistemaTarefa",
                claims: null,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credencials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}