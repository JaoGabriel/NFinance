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

        [ForeignKey("Id")] 
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
        [Range(typeof(DateTime), "01/01/1950", "31/12/2100", ErrorMessage = "Data {0} deve estar entre {1} e {2}")]
        public DateTime DataDoGanho { get; set; }

        public Ganho(Guid idCliente, string nomeGanho, decimal valor, bool recorrente, DateTime dataDoGanho)
        {
            ValidaCadastroGanho(idCliente,nomeGanho,valor,dataDoGanho);
            
            Id = Guid.NewGuid();
            IdCliente = idCliente;
            NomeGanho = nomeGanho;
            Valor = valor;
            Recorrente = recorrente;
            DataDoGanho = dataDoGanho;
        }

        public void AtualizaGanho(string nomeGanho, decimal valor, bool recorrente, DateTime dataDoGanho)
        {
            ValidaGanho(nomeGanho,valor,dataDoGanho);
            
            NomeGanho = nomeGanho;
            Valor = valor;
            Recorrente = recorrente;
            DataDoGanho = dataDoGanho;
        }

        private static void ValidaCadastroGanho(Guid idCliente, string nomeGanho, decimal valor, DateTime dataDoGanho)
        {
            if (Guid.Empty.Equals(idCliente)) 
                throw new DomainException();
            
            if (string.IsNullOrWhiteSpace(nomeGanho)) 
                throw new DomainException();
            
            if (valor is <= decimal.MinValue or >= decimal.MaxValue or <= decimal.Zero)
                throw new DomainException();

            if (dataDoGanho < DateTime.MinValue.AddYears(1949) || dataDoGanho > DateTime.MaxValue.AddYears(-7899))
                throw new DomainException();
        }
        
        private static void ValidaGanho(string nomeGanho, decimal valor, DateTime dataDoGanho)
        {
            if (string.IsNullOrWhiteSpace(nomeGanho)) 
                throw new DomainException();
            
            if (valor is <= decimal.MinValue or >= decimal.MaxValue or <= decimal.Zero)
                throw new DomainException();

            if (dataDoGanho <= DateTime.MinValue || dataDoGanho >= DateTime.MaxValue)
                throw new DomainException();
        }
    }
}