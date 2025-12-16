using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaTarefa.Models;

namespace SistemaTarefa.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<List<UsuarioModel>> GetAll();
        Task<UsuarioModel> GetByID(int id);
        Task<UsuarioModel> GetByEmail(string email);
        Task<UsuarioModel> Create(UsuarioModel usuario);

        Task<UsuarioModel> Update(UsuarioModel usuario, int id);

        Task<bool> Delete(int id);

    }
}