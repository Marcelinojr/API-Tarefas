using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaTarefa.Models;

namespace SistemaTarefa.Data.Map
{
    public class UsuarioMap :IEntityTypeConfiguration<UsuarioModel>    
    {

        public void Configure(EntityTypeBuilder<UsuarioModel> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Nome).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(150);
        }
        
    }
}