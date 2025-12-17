using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaTarefa.Models
{
    public class AuthModel
    {
        public int Id { get; set; }
        public int? UsuarioId { get; set; }

        public string? Token { get; set; }
    }
}