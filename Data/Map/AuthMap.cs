using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaTarefa.Models;

namespace SistemaTarefa.Data.Map
{
    public class AuthMap : IEntityTypeConfiguration<AuthModel>    
    {

        public void Configure(EntityTypeBuilder<AuthModel> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.UsuarioId).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Token).IsRequired().HasMaxLength(255);
        }
        
    }
}