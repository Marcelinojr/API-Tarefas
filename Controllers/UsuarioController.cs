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
using SistemaTarefa.Services.Interfaces;

namespace SistemaTarefa.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioService _usuarioService;
        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }


        [HttpGet]
        public async Task<ActionResult<List<UsuarioResponseDTO>>> GetAll()
        {

            // repository retorna lista de models
            var usuarios = await _usuarioService.GetAllAsync();

            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponseDTO>> GetByID(int id)
        {

            var usuarios = await _usuarioService.GetByIdAsync(id);

            return Ok(usuarios);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioCreateDTO>> Create([FromBody] UsuarioCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var usuario = await _usuarioService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetByID), new { id = usuario.Id }, usuario);
            }
            catch (Exception ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UsuarioUpdateDTO>> Update([FromBody] UsuarioUpdateDTO dto, int id)
        {
            // Validacao
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var usuario = await _usuarioService.UpdateAsync(id, dto);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/senha")]
        public async Task<ActionResult> AlterarSenha(int id, [FromBody] UsuarioPasswordDTO dto)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);

            try
            {
                await _usuarioService.AlterarSenhaAsync(id, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);

            if (usuario == null)
            {
                return NotFound(new { message = "Usuario nao encontrado" });
            }
            await _usuarioService.DeleteAsync(id);
            return NoContent();
        }
    }
}