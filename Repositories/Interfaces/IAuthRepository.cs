using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaTarefa.Models;

namespace SistemaTarefa.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<AuthModel> GetByUsuarioId(int usuarioId);

        Task<bool> ValidateToken(string token);

        Task<AuthModel> CreateToken(AuthModel auth);

        Task<AuthModel> UpdateToken(AuthModel auth, int id);

    }
}