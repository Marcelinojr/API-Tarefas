using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaTarefa.DTOs;

namespace SistemaTarefa.Services.Interfaces
{
    public interface ITarefaService
    {
        Task<TarefaResponseDTO> GetByIdAsync(int id);
        Task<List<TarefaResponseDTO>> GetAllAsync();
        Task<TarefaResponseDTO> CreateAsync(TarefaCreateDTO dto);
        Task<TarefaResponseDTO> UpdateAsync(int id, TarefaUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}