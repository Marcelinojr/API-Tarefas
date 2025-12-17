using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SistemaTarefa.Enums;

namespace SistemaTarefa.DTOs
{

    public class AuthResponseDTO
    {
        public string? Token { get; set; }

    }

    public class AuthLoginDTO
    {
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email fornecido não é válido.")]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class AuthDTO
    {

        public int Id { get; set; }
        public int? UsuarioId { get; set; }

        public string? Token { get; set; }

    }

}