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
    public class TarefaRepository : ITarefaRepository
    {
        // Dependency Injection of the database context
        private readonly SistemaTarefasDBContext _dbcontext;

        // Constructor
        public TarefaRepository(SistemaTarefasDBContext context)
        {
            _dbcontext = context;
        }



        // Implementing the methods from ITarefaRepository interface
        public async Task<TarefaModel> GetByID(int id)
        {
            return await _dbcontext.Tarefas
            .Include(u => u.Usuario)
            .FirstOrDefaultAsync(t => t.Id == id);
        }


        public async Task<List<TarefaModel>> GetAll()
        {
            return await _dbcontext.Tarefas
            .Include(u => u.Usuario)
            .ToListAsync();
        }

        public async Task<TarefaModel> Create(TarefaModel tarefa)
        {
            await _dbcontext.Tarefas.AddAsync(tarefa);
            await _dbcontext.SaveChangesAsync();
            return tarefa;
        }

        public async Task<TarefaModel> Update(TarefaModel tarefa, int id)
        {
            // Verify if the user exists
            TarefaModel tarefaPorId = await GetByID(id);
            if (tarefaPorId == null)
            {
                throw new Exception($"Tarefa para o ID: {id} não foi encontrado no banco de dados.");
            }

            // Update the user fields
            tarefaPorId.Nome = tarefa.Nome;
            tarefaPorId.Descricao = tarefa.Descricao;
            tarefaPorId.Status = tarefa.Status;
            tarefaPorId.UsuarioId = tarefa.UsuarioId;

            // Save changes to the database
            _dbcontext.Tarefas.Update(tarefaPorId);
            await _dbcontext.SaveChangesAsync();

            return tarefaPorId;
        }

        public async Task<bool> Delete(int id)
        {
            TarefaModel tarefaPorId = await GetByID(id);
            if (tarefaPorId == null)
            {
                throw new Exception($"Tarefa para o ID: {id} não foi encontrado no banco de dados.");
            }

            _dbcontext.Tarefas.Remove(tarefaPorId);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

    }
}