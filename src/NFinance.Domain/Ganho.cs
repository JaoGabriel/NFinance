using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NFinance.Domain.Exceptions;

namespace NFinance.Domain
{
    public class Ganho
    {
        [Key] 
        [Required] 
        public Guid Id { get; set; }

        [ForeignKey(nameof(Cliente))] 
        [Required] 
        public Guid IdCliente { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string NomeGanho { get; set; }

        [Required]
        [Range(0, 999999999999, ErrorMessage = "Valor {0} deve estar entre {1} e {2}")]
        public decimal Valor { get; set; }

        [Required] 
        public bool Recorrente { get; set; }

        [Required]
        [Range(typeof(DateTimeOffset), "01/01/1950", "31/12/2100", ErrorMessage = "Data {0} deve estar entre {1} e {2}")]
        public DateTimeOffset DataDoGanho { get; set; }

        public Ganho(Guid idCliente, string nomeGanho, decimal valor, bool recorrente, DateTimeOffset dataDoGanho)
        {
            ValidaDadosCadastroGanho(idCliente,nomeGanho,valor,dataDoGanho);
            
            Id = Guid.NewGuid();
            IdCliente = idCliente;
            NomeGanho = nomeGanho;
            Valor = valor;
            Recorrente = recorrente;
            DataDoGanho = dataDoGanho;
        }

        public void AtualizaGanho(string nomeGanho, decimal valor, bool recorrente, DateTimeOffset dataDoGanho)
        {
            ValidaDadosAtualizacaoGanho(nomeGanho,valor,dataDoGanho);
            
            NomeGanho = nomeGanho;
            Valor = valor;
            Recorrente = recorrente;
            DataDoGanho = dataDoGanho;
        }

        private static void ValidaDadosCadastroGanho(Guid idCliente, string nomeGanho, decimal valor, DateTimeOffset dataDoGanho)
        {
            if (Guid.Empty.Equals(idCliente)) 
                throw new DomainException("Cliente invalido ou inexistente.");
            
            if (string.IsNullOrWhiteSpace(nomeGanho)) 
                throw new DomainException("Nome invalido.");
            
            if (valor is <= decimal.MinValue or >= decimal.MaxValue or <= decimal.Zero)
                throw new DomainException("Valor invalido.");

            if (dataDoGanho <= DateTimeOffset.MinValue || dataDoGanho >= DateTimeOffset.MaxValue)
                throw new DomainException("Data invalida.");
        }
        
        private static void ValidaDadosAtualizacaoGanho(string nomeGanho, decimal valor, DateTimeOffset dataDoGanho)
        {
            if (string.IsNullOrWhiteSpace(nomeGanho)) 
                throw new DomainException("Nome invalido.");
            
            if (valor is <= decimal.MinValue or >= decimal.MaxValue or <= decimal.Zero)
                throw new DomainException("Valor invalido.");

            if (dataDoGanho <= DateTimeOffset.MinValue || dataDoGanho >= DateTimeOffset.MaxValue)
                throw new DomainException("Data invalida.");
        }
    }
}