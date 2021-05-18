using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NFinance.Domain
{
    public class Gasto
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [ForeignKey("Id")]
        [Required]
        public Guid IdCliente { get; set; }

        [Required(ErrorMessage = "Nome do Gasto deve conter no minimo 10 letras e no maximo 100")]
        [StringLength(100, MinimumLength = 10)]
        public string NomeGasto { get; set; }

        [Required]
        [Range(0, 999999999999, ErrorMessage = "Valor {0} deve estar entre {1} e {2}")]
        public decimal Valor { get; set; }
        
        [Required]
        [Range(-1, 1000, ErrorMessage = "Sua parcela {0} deve estar entre {1} e {2}")]
        public int QuantidadeParcelas { get; set; }
        
        [Required]
        [Range(typeof(DateTime), "01/01/1950", "12/31/2100", ErrorMessage = "Data {0} deve estar entre {1} e {2}")]
        public DateTime DataDoGasto { get; set; }

        public Gasto() { }

        public Gasto(Guid idCliente, string nomeGasto, decimal valor, int quantidadeParcelas, DateTime dataDoGasto)
        {
            Id = Guid.NewGuid();
            IdCliente = idCliente;
            NomeGasto = nomeGasto;
            Valor = valor;
            QuantidadeParcelas = quantidadeParcelas;
            DataDoGasto = dataDoGasto;
        }

        public Gasto(Guid id, Guid idCliente, string nomeGasto, decimal valor, int quantidadeParcelas, DateTime dataDoGasto)
        {
            Id = id;
            IdCliente = idCliente;
            NomeGasto = nomeGasto;
            Valor = valor;
            QuantidadeParcelas = quantidadeParcelas;
            DataDoGasto = dataDoGasto;
        }
    }
}
