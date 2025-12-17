using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SistemaTarefa.Enums;

namespace SistemaTarefa.DTOs
{

    public class TarefaResponseDTO
    {
        public int Id { get; set; }
        public string? Nome { get; set; }

        public string? Descricao { get; set; }

        public StatusTarefa Status { get; set; }

        public int? UsuarioId { get; set; }
        public virtual UsuarioResponseDTO? Usuario { get; set; }

    }


    public class TarefaCreateDTO
    {

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string? Nome { get; set; }


        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres.")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O status é obrigatório.")]
        [EnumDataType(typeof(StatusTarefa), ErrorMessage = "Status inválido.")]
        public StatusTarefa Status { get; set; }

        [Required(ErrorMessage = "O id do usuário é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "Id do usuário inválido.")]
        public int? UsuarioId { get; set; }

    }

    public class TarefaUpdateDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string? Nome { get; set; }


        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres.")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O status é obrigatório.")]
        [EnumDataType(typeof(StatusTarefa), ErrorMessage = "Status inválido.")]
        public StatusTarefa Status { get; set; }

        [Required(ErrorMessage = "O id do usuário é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "Id do usuário inválido.")]
        public int? UsuarioId { get; set; }

    }

}