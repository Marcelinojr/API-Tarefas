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


            return auth;
        }

        public async Task<AuthModel> UpdateToken(AuthModel auth, int id)
        {
            var existingAuth = await _dbcontext.Auth
            .FirstOrDefaultAsync(a => a.UsuarioId == id);
            if (existingAuth == null)
            {
                throw new Exception($"usuarioId: {id} não possui token associado.");
            }

            existingAuth.Token = auth.Token;
            _dbcontext.Auth.Update(existingAuth);
            await _dbcontext.SaveChangesAsync();

            return existingAuth;
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