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
    public class TarefaController : ControllerBase
    {

        private readonly ITarefaService _tarefaService;
        public TarefaController(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }


        [HttpGet]
        public async Task<ActionResult<List<TarefaResponseDTO>>> GetAllTarefas()
        {

            List<TarefaResponseDTO> tarefas = await _tarefaService.GetAllAsync();
            return Ok(tarefas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TarefaResponseDTO>> GetByID(int id)
        {
            TarefaResponseDTO tarefa = await _tarefaService.GetByIdAsync(id);
            return Ok(tarefa);
        }

        [HttpPost]
        public async Task<ActionResult<TarefaResponseDTO>> Create([FromBody] TarefaCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TarefaResponseDTO tarefa = await _tarefaService.CreateAsync(dto);

            return Ok(tarefa);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TarefaResponseDTO>> Update([FromBody] TarefaUpdateDTO dto, int id)
        {
            TarefaResponseDTO tarefa = await _tarefaService.UpdateAsync(id, dto);
            return Ok(tarefa);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _tarefaService.DeleteAsync(id);
            return NoContent();
        }
    }
}