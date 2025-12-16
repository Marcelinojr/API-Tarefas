using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaTarefa.Models;

namespace SistemaTarefa.Repositories.Interfaces
{
    public interface ITarefaRepository
    {
        Task<List<TarefaModel>> GetAll();
        Task<TarefaModel> GetByID(int id);
        Task<TarefaModel> Create(TarefaModel tarefa);

        Task<TarefaModel> Update(TarefaModel tarefa, int id);

        Task<bool> Delete(int id);

    }
}