using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using NFinance.Domain.Exceptions;

namespace NFinance.Domain
{
    public class Investimento
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Cliente))]
        [Required]
        public Guid IdCliente { get; set; }

        [Required(ErrorMessage = "Nome do Investimento deve conter no minimo 10 letras e no maximo 100")]
        [StringLength(100, MinimumLength = 10)]
        public string NomeInvestimento { get; set; }

        [Required]
        [Range(0, 999999999999, ErrorMessage = "Valor {0} deve estar entre {1} e {2}")]
        public decimal Valor { get; set; }
        
        [Required]
        [Range(typeof(DateTimeOffset),"01/01/1950","12/31/2100", ErrorMessage = "Data {0} deve estar entre {1} e {2}")]
        public DateTimeOffset DataAplicacao { get; set; }

        public Investimento(Guid idCliente, string nomeInvestimento, decimal valor, DateTimeOffset dataAplicacao)
        {
            ValidaInformacoesInvestimento(nomeInvestimento, valor, dataAplicacao);
            
            Id = Guid.NewGuid();
            IdCliente = idCliente;
            NomeInvestimento = nomeInvestimento;
            Valor = valor;
            DataAplicacao = dataAplicacao;
        }

        public void AtualizaInvestimento(string nomeInvestimento, decimal valor, DateTimeOffset dataAplicacao)
        {
            ValidaInformacoesInvestimento(nomeInvestimento, valor, dataAplicacao);
            
            NomeInvestimento = nomeInvestimento;
            Valor = valor;
            DataAplicacao = dataAplicacao;
        }

        private static void ValidaInformacoesInvestimento(string nomeInvestimento, decimal valor, DateTimeOffset dataAplicacao)
        {
            if (string.IsNullOrWhiteSpace(nomeInvestimento)) 
                throw new DomainException("Nome Inválido.");

            if (valor is <= decimal.MinValue or >= decimal.MaxValue or <= decimal.Zero)
                throw new DomainException("Valor Inválido");
            
            if (dataAplicacao <= DateTimeOffset.MinValue || dataAplicacao >= DateTimeOffset.MaxValue)
                throw new DomainException("Data Inválida.");
        }
    }
}
