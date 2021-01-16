using System.ComponentModel.DataAnnotations;

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
        [Range(typeof(decimal), "0", "999999999999", ErrorMessage = "Valor {0} deve estar entre {1} e {2}")]
        public decimal RendaMensal { get; set; }
    }
}
