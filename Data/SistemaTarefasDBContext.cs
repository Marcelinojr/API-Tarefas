using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaTarefa.Data.Map;
using SistemaTarefa.Models;

namespace SistemaTarefa.Data
{
    public class SistemaTarefasDBContext : DbContext
    {
        public SistemaTarefasDBContext(DbContextOptions<SistemaTarefasDBContext> options) : base(options)
        {
            
        }

        // Definindo as tabelas do banco de dados
        public DbSet<UsuarioModel> Usuarios {get; set ;}
        public DbSet<TarefaModel> Tarefas {get; set ;}

        public DbSet<AuthModel> Auth { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new TarefaMap());
            // modelBuilder.ApplyConfiguration(new AuthMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}