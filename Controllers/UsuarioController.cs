using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaTarefa.DTOs;
using SistemaTarefa.Models;
using SistemaTarefa.Repositories.Interfaces;

namespace SistemaTarefa.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }


        [HttpGet]
        public async Task<ActionResult<List<UsuarioResponseDTO>>> GetAll()
        {

            // repository retorna lista de models
            var usuarios = await _usuarioRepository.GetAll();

            // converte manualmente a lista de models para lista de DTOs
            var usuariosDTO = usuarios.Select(u => new UsuarioResponseDTO
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email
                //nao vamos incluir a senha no DTO de resposta
            }).ToList();

            return Ok(usuariosDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponseDTO>> GetByID(int id)
        {
            //Receber do models e converter para DTO
            var usuarios = await _usuarioRepository.GetByID(id);
            if(usuarios == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }
            var usuarioDTO = new UsuarioResponseDTO
            {
                Id = usuarios.Id,
                Nome = usuarios.Nome,
                Email = usuarios.Email
            };

            return Ok(usuarioDTO);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioCreateDTO>> Create([FromBody] UsuarioCreateDTO usuarioCreateDTO)
        {
            //Validar com base modelo de dados especificado no parametros
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //validar se o email ja existe
            var usuarioExistente = await _usuarioRepository.GetByEmail(usuarioCreateDTO.Email);
            if (usuarioExistente != null)
            {
                return Conflict("Email já cadastrado.");
            }

            // Converter DTO para Model
            var usuarioModel = new UsuarioModel
            {
                Nome = usuarioCreateDTO.Nome,
                Email = usuarioCreateDTO.Email,
                Senha = BCrypt.Net.BCrypt.HashPassword(usuarioCreateDTO.Senha)
            };

            // Converter Model para ResponseDTO 
            var usuarioResponseDTO = new UsuarioResponseDTO
            {
                Id = usuarioModel.Id,
                Nome = usuarioModel.Nome,
                Email = usuarioModel.Email
            };

            // Chamar o repositório para criar o usuário
            UsuarioModel usuario = await _usuarioRepository.Create(usuarioModel);

            return CreatedAtAction(nameof(GetByID), new { id = usuario.Id }, usuarioResponseDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UsuarioUpdateDTO>> Update([FromBody] UsuarioUpdateDTO usuarioUpdateDTO, int id)
        {
            // Validacao
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            //validar se o user existe
            var usuarioExistente = await _usuarioRepository.GetByID(id);
            if(usuarioExistente == null){
                return NotFound("Usuário não encontrado.");
            }

            // Converter DTO para Model
            // var usuarioModel = new UsuarioModel
            // {
            //     Nome = usuarioUpdateDTO.Nome,
            //     Email = usuarioUpdateDTO.Email,
            // };
            usuarioExistente.Nome = usuarioUpdateDTO.Nome;
            usuarioExistente.Email = usuarioUpdateDTO.Email;
            

            // Chamar o repositório para atualizar o usuário
            UsuarioModel usuarioAtualizado = await _usuarioRepository.Update(usuarioExistente, id);

            // Converter Model para ResponseDTO 
            var responseDTO = new UsuarioResponseDTO{
                Id = usuarioAtualizado.Id,
                Nome = usuarioAtualizado.Nome,
                Email = usuarioAtualizado.Email
            };
            return Ok(responseDTO);
        }
        [HttpPut("{id}/senha")]
        public async Task<ActionResult> AlterarSenha(int id, [FromBody] UsuarioPasswordDTO usuarioPasswordDTO){
            var usuario = await _usuarioRepository.GetByID(id);

            if(usuario == null){
                return NotFound(new {message = "Usuario nao encontrado"});
            }
            if(!BCrypt.Net.BCrypt.Verify(usuarioPasswordDTO.SenhaAtual, usuario.Senha)){
                return BadRequest(new {message = "Senha atual incorreta"});
            }



            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuarioPasswordDTO.NovaSenha);
            await _usuarioRepository.UpdateSenha(usuario, id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var usuario = await _usuarioRepository.GetByID(id);

            if(usuario == null){
                return NotFound(new {message = "Usuario nao encontrado"});
            }
            await _usuarioRepository.Delete(id);
            return NoContent();
        }
    }
}