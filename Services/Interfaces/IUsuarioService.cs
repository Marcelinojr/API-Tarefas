using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaTarefa.DTOs;

namespace SistemaTarefa.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioResponseDTO> GetByIdAsync(int id);
        Task<List<UsuarioResponseDTO>> GetAllAsync();
        Task<UsuarioResponseDTO> CreateAsync(UsuarioCreateDTO dto);
        Task<UsuarioResponseDTO> UpdateAsync(int id, UsuarioUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> AlterarSenhaAsync(int id, UsuarioPasswordDTO dto);
    }
}