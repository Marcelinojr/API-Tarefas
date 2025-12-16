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
using SistemaTarefa.Repositories.Interfaces;

namespace SistemaTarefa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        private readonly IConfiguration _configuration;
        public AuthController(IAuthRepository authRepository, IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<AuthModel>> Login([FromBody] LoginRequestModel loginRequest)
        {
            var validateCredentials = await _usuarioRepository.GetByEmail(loginRequest.Email);
            if (validateCredentials == null || validateCredentials.Senha != loginRequest.Password)
            {
                return BadRequest("Email ou senha inválidos.");
            }

            var existingAuth = await _authRepository.GetByUsuarioId(validateCredentials.Id);
            var token = string.Empty;

            if (existingAuth == null)
            {
                token = await CreateAuth(validateCredentials);
                return Ok(token);
            }
            else
            {
                var checkValidToken = await _authRepository.ValidateToken(existingAuth.Token);

                if (!checkValidToken)
                {
                    token = await CreateAuth(validateCredentials);
                    return Ok(token);
                }
                token = await UpdateAuth(validateCredentials);

                return Ok(token);
            }



        }

        private async Task<bool> ValidateToken()
        {
            throw new NotImplementedException();
        }

        private async Task<string> CreateAuth(UsuarioModel validateCredentials)
        {
            AuthModel authModel = new AuthModel();

            authModel.UsuarioId = validateCredentials.Id;
            authModel.Token = GerarTokenJwt();

            await _authRepository.CreateToken(authModel);

            return authModel.Token;
        }

        private async Task<string> UpdateAuth(UsuarioModel validateCredentials)
        {
            AuthModel authModel = new AuthModel();

            string token = GerarTokenJwt();
            authModel.UsuarioId = validateCredentials.Id;
            authModel.Token = token;

            await _authRepository.UpdateToken(authModel, validateCredentials.Id);


            return token;
        }


        private string GerarTokenJwt()
        {

            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credencials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("role","admin"),
                new Claim("username","Administrador")
            };

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credencials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}