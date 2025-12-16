using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaTarefa.Data;
using SistemaTarefa.Models;

namespace SistemaTarefa.Repositories.Interfaces
{
    public class AuthRepository : IAuthRepository
    {
        private readonly SistemaTarefasDBContext _dbcontext;

        public AuthRepository(SistemaTarefasDBContext context)
        {
            _dbcontext = context;
        }

        public async Task<AuthModel> CreateToken(AuthModel auth)
        {

           await _dbcontext.Auth.AddAsync(auth);
           await _dbcontext.SaveChangesAsync();
           
           return auth;
        }

        public async Task<AuthModel> GetByUsuarioId(int usuarioId)
        {
            AuthModel auth = await _dbcontext.Auth
            .FirstOrDefaultAsync(a => a.UsuarioId == usuarioId);
            if (auth == null)
            {
                throw new Exception($"Token para o UsuarioId: {usuarioId}.");
            }

            return auth;
        }

        public async Task<AuthModel> UpdateToken(AuthModel auth, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidateToken(string token)
        {
            var auth = await _dbcontext.Auth
            .FirstOrDefaultAsync(a => a.Token == token);
            if (auth == null)
            {
                return false;
            }

            return true;
        }
    }

}