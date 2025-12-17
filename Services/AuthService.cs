using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using SistemaTarefa.DTOs;
using SistemaTarefa.Models;
using SistemaTarefa.Repositories.Interfaces;
using SistemaTarefa.Services.Interfaces;

namespace SistemaTarefa.Services
{
    public class AuthService : IAuthService
    {

        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly IUsuarioRepository _usuarioRepository;
        public AuthService(IAuthRepository authRepository, IConfiguration configuration, IUsuarioRepository usuarioRepository)
        {
            _authRepository = authRepository;
            _configuration = configuration;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<AuthResponseDTO> Authenticate(AuthLoginDTO dto)
        {
            //Validar credentials
            var currentUser = await _usuarioRepository.GetByEmail(dto.Email);

            if (currentUser == null || currentUser.Senha != dto.Password)
            {
                return null;
            }

            //Validar se tem token
            var auth = await ValidateToken(dto);

            //Enviar o existente ou autenticar
            var newAuthDTO = new AuthDTO
            {
                UsuarioId = currentUser.Id,
                Token = GerarTokenJwt()
            };
            if (auth)
            {
                var Updatedtoken = await UpdateToken(newAuthDTO);
                return Updatedtoken;
            }
             var newToken = await CreateToken(newAuthDTO);
            return newToken;
        }

        public async Task<AuthResponseDTO> CreateToken(AuthDTO dto)
        {

            var newToken = ToModel(dto);
            await _authRepository.CreateToken(newToken);

            return ToResponseDTO(newToken);
        }

        public async Task<AuthResponseDTO> UpdateToken(AuthDTO dto)
        {

            var newToken = ToModel(dto);
            await _authRepository.CreateToken(newToken);

            return ToResponseDTO(newToken);
        }

        public async Task<bool> ValidateToken(AuthLoginDTO dto)
        {
            var user = await _usuarioRepository.GetByEmail(dto.Email);
            if (user == null)
            {
                return false;
            }
            var existingAuth = await _authRepository.GetByUsuarioId(user.Id);

            if (existingAuth == null)
            {
                return false;
            }

            return true;
        }


        // Methods

        private AuthResponseDTO ToResponseDTO(AuthModel model)
        {
            return new AuthResponseDTO
            {
                Token = model.Token
            };
        }

        private AuthModel ToModel(AuthDTO dto)
        {

            return new AuthModel
            {
                UsuarioId = dto.UsuarioId,
                Token = GerarTokenJwt()
            };

        }


        private string GerarTokenJwt()
        {
            var jwtKey = _configuration["Jwt:Key"];

            if (jwtKey == null)
            {
                throw new NotImplementedException();
            }


            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
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