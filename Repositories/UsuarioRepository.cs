using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaTarefa.Data;
using SistemaTarefa.Models;
using SistemaTarefa.Repositories.Interfaces;

namespace SistemaTarefa.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        // Dependency Injection of the database context
        private readonly SistemaTarefasDBContext _dbcontext;

        // Constructor
        public UsuarioRepository(SistemaTarefasDBContext context)
        {
            _dbcontext = context;
        }



        // Implementing the methods from IUsuarioRepository interface
        public async Task<UsuarioModel> GetByID(int id)
        {
            return await _dbcontext.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        }


        public async Task<List<UsuarioModel>> GetAll()
        {
            return await _dbcontext.Usuarios.ToListAsync();
        }

        public async Task<UsuarioModel> Create(UsuarioModel usuario)
        {
            await _dbcontext.Usuarios.AddAsync(usuario);
            await _dbcontext.SaveChangesAsync();
            return usuario;
        }

        public async Task<UsuarioModel> Update(UsuarioModel usuario, int id)
        {
            // Verify if the user exists
            UsuarioModel usuarioPorId = await GetByID(id);
            if (usuarioPorId == null)
            {
                throw new Exception($"Usuário para o ID: {id} não foi encontrado no banco de dados.");
            }

            // Update the user fields
            usuarioPorId.Nome = usuario.Nome;
            usuarioPorId.Email = usuario.Email;

            // Save changes to the database
            _dbcontext.Usuarios.Update(usuarioPorId);
            await _dbcontext.SaveChangesAsync();

            return usuarioPorId;
        }

        public async Task<bool> Delete(int id)
        {
            UsuarioModel usuarioPorId = await GetByID(id);
            if (usuarioPorId == null)
            {
                throw new Exception($"Usuário para o ID: {id} não foi encontrado no banco de dados.");
            }

            _dbcontext.Usuarios.Remove(usuarioPorId);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

    }
}