using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NFinance.Domain
{
    public class Cliente
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [Range(0, 999999999999, ErrorMessage = "Valor {0} deve estar entre {1} e {2}")]
        public decimal RendaMensal { get; set; }

        [ForeignKey("Id")]
        public Investimentos Investimentos { get; set; }

        [ForeignKey("Id")]
        public Gastos Gastos { get; set; }
    }
}
