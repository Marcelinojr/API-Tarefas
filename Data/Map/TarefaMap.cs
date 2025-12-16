using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaTarefa.Models;

namespace SistemaTarefa.Data.Map
{
    public class TarefaMap :IEntityTypeConfiguration<TarefaModel>    
    {

        public void Configure(EntityTypeBuilder<TarefaModel> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Nome).IsRequired().HasMaxLength(255);
            builder.Property(u => u.Descricao).HasMaxLength(250);
            builder.Property(u => u.Status).IsRequired();
        }
        
    }
}