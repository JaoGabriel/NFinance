using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NFinance.Domain.Exceptions;

namespace NFinance.Domain
{
    public class Resgate
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Investimento))]
        [Required]
        public Guid IdInvestimento { get; set; }

        [ForeignKey(nameof(Cliente))]
        [Required]
        public Guid IdCliente { get; set; }

        [Required]
        [Range(0 ,999999999999, ErrorMessage = "Valor {0} deve estar entre {1} e {2}")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Motivo deve conter no minimo 10 letras e no maximo 100")]
        [StringLength(100, MinimumLength = 10)]
        public string MotivoResgate { get; set; }

        [Required]
        [Range(typeof(DateTimeOffset), "01/01/1950", "12/31/2100", ErrorMessage = "Data {0} deve estar entre {1} e {2}")]
        public DateTimeOffset DataResgate { get; set; }

        public Resgate(Guid idInvestimento, Guid idCliente, decimal valor, string motivoResgate, DateTimeOffset dataResgate)
        {
            ValidaDadosResgate(idInvestimento,idCliente,valor,motivoResgate,dataResgate);
            
            Id = Guid.NewGuid();
            IdInvestimento = idInvestimento;
            IdCliente = idCliente;
            Valor = valor;
            MotivoResgate = motivoResgate;
            DataResgate = dataResgate;
        }

        public void AtualizaResgate(decimal valor, string motivoResgate, DateTimeOffset dataResgate)
        {
            ValidaDadosAtualizacaoResgate(valor,motivoResgate,dataResgate);
            
            Valor = valor;
            MotivoResgate = motivoResgate;
            DataResgate = dataResgate;
        }

        private static void ValidaDadosResgate(Guid idInvestimento, Guid idCliente, decimal valor, string motivoResgate, DateTimeOffset dataResgate)
        {
            if (Guid.Empty.Equals(idInvestimento)) 
                throw new DomainException();
            
            if (Guid.Empty.Equals(idCliente)) 
                throw new DomainException();
            
            if (string.IsNullOrWhiteSpace(motivoResgate)) 
                throw new DomainException();
            
            if (valor is <= decimal.MinValue or >= decimal.MaxValue or <= decimal.Zero)
                throw new DomainException();

            if (dataResgate <= DateTimeOffset.MinValue || dataResgate >= DateTimeOffset.MaxValue)
                throw new DomainException();
        }

        private static void ValidaDadosAtualizacaoResgate(decimal valor, string motivoResgate, DateTimeOffset dataResgate)
        {
            if (string.IsNullOrWhiteSpace(motivoResgate)) 
                throw new DomainException();
            
            if (valor is <= decimal.MinValue or >= decimal.MaxValue or <= decimal.Zero)
                throw new DomainException();

            if (dataResgate <= DateTimeOffset.MinValue || dataResgate >= DateTimeOffset.MaxValue)
                throw new DomainException();
        }
    }
}
