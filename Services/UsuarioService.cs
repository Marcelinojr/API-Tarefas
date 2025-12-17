using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaTarefa.DTOs;
using SistemaTarefa.Models;
using SistemaTarefa.Services.Interfaces;
using SistemaTarefa.Repositories.Interfaces;

namespace SistemaTarefa.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<UsuarioResponseDTO> CreateAsync(UsuarioCreateDTO dto)
        {
            var usuarioExistente = await _usuarioRepository.GetByEmail(dto.Email);
            if (usuarioExistente != null)
            {
                return null;
            }

            var novoUsuario = new UsuarioModel
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = BCrypt.Net.BCrypt.HashPassword(dto.Senha)// Em um cenário real, a senha deve ser hashada antes de ser armazenada
            };

            var usuarioCriado = await _usuarioRepository.Create(novoUsuario);
            return ToResponseDTO(usuarioCriado);
        }


        public async Task<List<UsuarioResponseDTO>> GetAllAsync()
        {
            var usuarios = await _usuarioRepository.GetAll();

            return usuarios.Select(u => ToResponseDTO(u)).ToList();
        }

        public async Task<UsuarioResponseDTO> GetByIdAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByID(id);
            if (usuario == null)
            {
                return null;
            }

            return ToResponseDTO(usuario);
        }

        public async Task<UsuarioResponseDTO> UpdateAsync(int id, UsuarioUpdateDTO dto)
        {
            var usuario = await _usuarioRepository.GetByID(id);
            if (usuario == null)
            {
                return null;
            }

            usuario.Nome = dto.Nome;
            usuario.Email = dto.Email;

            var usuarioAtualizado = await _usuarioRepository.Update(usuario, id);

            return ToResponseDTO(usuarioAtualizado);
        }

        public async Task<bool> AlterarSenhaAsync(int id, UsuarioPasswordDTO dto)
        {
            var usuario = _usuarioRepository.GetByID(id).Result;
            if (usuario == null)
            {
                return false;
            }
            if (!BCrypt.Net.BCrypt.Verify(dto.SenhaAtual, usuario.Senha))
            {
                return false;
            }

            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(dto.SenhaAtual);
            await _usuarioRepository.UpdateSenha(usuario, id);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByID(id);
            if (usuario == null)
            {
                return false;
            }
            await _usuarioRepository.Delete(id);
            return true;
        }


        private UsuarioResponseDTO ToResponseDTO(UsuarioModel model)
        {
            return new UsuarioResponseDTO
            {
                Id = model.Id,
                Nome = model.Nome,
                Email = model.Email
            };
        }

    }
}