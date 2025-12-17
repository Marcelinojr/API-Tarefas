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
                throw new Exception($"Usuário para o ID: {id} não foi encontrado.");
            }

            // Update the user fields
            usuarioPorId.Nome = usuario.Nome;
            usuarioPorId.Email = usuario.Email;
            usuarioPorId.Senha = usuario.Senha;

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
                throw new Exception($"Usuário para o ID: {id} não foi encontrado.");
            }

            _dbcontext.Usuarios.Remove(usuarioPorId);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<UsuarioModel> GetByEmail(string email)
        {
            var usuario = await _dbcontext.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null)
            {
                throw new Exception($"Usuário com o email: {email} não foi encontrado.");
            }
            
            return usuario;
        }

        public async Task<UsuarioModel> UpdateSenha(UsuarioModel usuario, int id)
        {
            var usuarioPorId = await _dbcontext.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (usuarioPorId == null)
            {
                throw new Exception($"Usuário para o ID: {id} não foi encontrado.");
            }

            usuarioPorId.Senha = usuario.Senha;

            _dbcontext.Usuarios.Update(usuarioPorId);
            await _dbcontext.SaveChangesAsync();

            return usuarioPorId;
        }
    }
}