using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NFinance.Domain.Exceptions;

namespace NFinance.Domain
{
    public class Gasto
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Cliente))]
        [Required]
        public Guid IdCliente { get; set; }

        [Required(ErrorMessage = "Nome do Gasto deve conter no minimo 5 letras e no maximo 100")]
        [StringLength(100, MinimumLength = 5)]
        public string NomeGasto { get; set; }

        [Required]
        [Range(0, 999999999999, ErrorMessage = "Valor {0} deve estar entre {1} e {2}")]
        public decimal Valor { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Sua parcela {0} deve estar entre {1} e {2}")]
        public int QuantidadeParcelas { get; set; }

        [Required]
        [Range(typeof(DateTimeOffset), "01/01/1950", "31/12/2100", ErrorMessage = "Data {0} deve estar entre {1} e {2}")]
        public DateTimeOffset DataDoGasto { get; set; }

        public Gasto(Guid idCliente, string nomeGasto, decimal valor, int quantidadeParcelas, DateTimeOffset dataDoGasto)
        {
            ValidaDadosGasto(idCliente,nomeGasto,valor,quantidadeParcelas,dataDoGasto);
            
            Id = Guid.NewGuid();
            IdCliente = idCliente;
            NomeGasto = nomeGasto;
            Valor = valor;
            QuantidadeParcelas = quantidadeParcelas;
            DataDoGasto = dataDoGasto;
        }

        public void AtualizaGasto(string nomeGasto, decimal valor, int quantidadeParcelas, DateTimeOffset dataDoGasto)
        {
            ValidaDadosAtualizacaoGasto(nomeGasto,valor,quantidadeParcelas,dataDoGasto);
            
            NomeGasto = nomeGasto;
            Valor = valor;
            QuantidadeParcelas = quantidadeParcelas;
            DataDoGasto = dataDoGasto;
        }

        private static void ValidaDadosGasto(Guid idCliente, string nomeGasto, decimal valor, int quantidadeParcelas, DateTimeOffset dataDoGasto)
        {
            if (Guid.Empty.Equals(idCliente)) 
                throw new DomainException("Cliente Inválido.");
            
            if (string.IsNullOrWhiteSpace(nomeGasto)) 
                throw new DomainException("Nome Inválido.");
            
            if (valor is <= decimal.MinValue or >= decimal.MaxValue or <= decimal.Zero)
                throw new DomainException("Valor Inválido.");

            if (quantidadeParcelas < 0)
                throw new DomainException("Quantidade de parcelas inválida.");

            if (dataDoGasto <= DateTimeOffset.MinValue || dataDoGasto >= DateTimeOffset.MaxValue)
                throw new DomainException("Data Inválida.");
        }

        private static void ValidaDadosAtualizacaoGasto(string nomeGasto, decimal valor, int quantidadeParcelas, DateTimeOffset dataDoGasto)
        {
            if (string.IsNullOrWhiteSpace(nomeGasto)) 
                throw new DomainException("Nome Inválido.");
            
            if (valor is <= decimal.MinValue or >= decimal.MaxValue or <= decimal.Zero)
                throw new DomainException("Valor Inválido.");

            if (quantidadeParcelas < 0)
                throw new DomainException("Quantidade de parcelas inválida.");

            if (dataDoGasto <= DateTimeOffset.MinValue || dataDoGasto >= DateTimeOffset.MaxValue)
                throw new DomainException("Data Inválida.");
        }
    }
}