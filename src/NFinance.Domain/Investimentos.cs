using System;
using System.ComponentModel.DataAnnotations;

namespace NFinance.Domain
{
    public class Investimentos
    {
        [Key]
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Nome { get; set; }
        
        [Required]
        [Range(typeof(decimal), "0", "999999999999", ErrorMessage = "Valor {0} deve estar entre {1} e {2}")]
        public decimal Valor { get; set; }
        
        [Required]
        [Range(typeof(DateTime),"01/01/1900","12/31/2060", ErrorMessage = "Data {0} deve estar entre {1} e {2}")]
        public DateTime DataAplicacao { get; set; }
    }
}
