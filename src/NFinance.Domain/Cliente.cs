using System;
using System.ComponentModel.DataAnnotations;

namespace NFinance.Domain
{
    public class Cliente
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Nome deve conter no minimo 10 letras e no maximo 100")]
        [StringLength(100,MinimumLength = 10)]
        public string Nome { get; set; }

        [Required]
        [Range(0, 999999999999, ErrorMessage = "Valor {0} deve estar entre {1} e {2}")]
        public decimal RendaMensal { get; set; }
    }
}
