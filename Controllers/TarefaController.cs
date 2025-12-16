using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaTarefa.Models;
using SistemaTarefa.Repositories.Interfaces;

namespace SistemaTarefa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {

        private readonly ITarefaRepository _tarefaRepository;
        public TarefaController(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }


        [HttpGet]
        public async Task<ActionResult<List<TarefaModel>>> GetAllTarefas()
        {

            List<TarefaModel> tarefas = await _tarefaRepository.GetAll();
            return Ok(tarefas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TarefaModel>> GetByID(int id)
        {
            TarefaModel tarefa = await _tarefaRepository.GetByID(id);
            return Ok(tarefa);
        }

        [HttpPost]
        public async Task<ActionResult<TarefaModel>> Create([FromBody] TarefaModel tarefaModel)
        {
            TarefaModel tarefa = await _tarefaRepository.Create(tarefaModel);
            return Ok(tarefa);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TarefaModel>> Update([FromBody] TarefaModel tarefaModel, int id)
        {
            TarefaModel tarefa = await _tarefaRepository.Update(tarefaModel, id);
            return Ok(tarefa);     
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _tarefaRepository.Delete(id);
            return NoContent();
        }
    }
}