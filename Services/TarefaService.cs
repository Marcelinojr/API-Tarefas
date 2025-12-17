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
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;

        public TarefaService(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }

        public async Task<TarefaResponseDTO> CreateAsync(TarefaCreateDTO dto)
        {
            var tarefas = await _tarefaRepository.Create(ToModel(dto));

            return ToResponseDTO(tarefas);
        }

        public async Task<List<TarefaResponseDTO>> GetAllAsync()
        {
            List<TarefaModel> tarefas = await _tarefaRepository.GetAll();
            List<TarefaResponseDTO> tarefasDTO = tarefas.Select(t => ToResponseDTO(t)).ToList();
            return tarefasDTO;
        }

        public async Task<TarefaResponseDTO> GetByIdAsync(int id)
        {
            TarefaModel tarefa = await _tarefaRepository.GetByID(id);
            return ToResponseDTO(tarefa);
        }

        public async Task<TarefaResponseDTO> UpdateAsync(int id, TarefaUpdateDTO dto)
        {
            TarefaModel tarefa = await _tarefaRepository.GetByID(id);

            tarefa.Nome = dto.Nome;
            tarefa.Descricao = dto.Descricao;
            tarefa.Status = dto.Status;
            tarefa.UsuarioId = dto.UsuarioId;

            await _tarefaRepository.Update(tarefa, id);
            return ToResponseDTO(tarefa);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            
            var tarefa = await _tarefaRepository.GetByID(id);

            if (tarefa == null)
            {
                return false;
            } 

            await _tarefaRepository.Delete(id);

            return true;

        }

        
        private TarefaResponseDTO ToResponseDTO(TarefaModel model)
        {
            return new TarefaResponseDTO
            {
                Id = model.Id,
                Nome = model.Nome,
                Descricao = model.Descricao,
                Status = model.Status,
                UsuarioId = model.UsuarioId,
                Usuario = model.Usuario != null ? new UsuarioResponseDTO
                {
                    Id = model.Usuario.Id,
                    Nome = model.Usuario.Nome,
                    Email = model.Usuario.Email
                } : null
            };
        }

        private TarefaModel ToModel(TarefaCreateDTO dto)
        {
            return new TarefaModel
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Status = dto.Status,
                UsuarioId = dto.UsuarioId
            };
        }

    }
}