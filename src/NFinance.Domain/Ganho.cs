using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public string MotivoExclusao { get; set; }

        public Ganho() {}

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

        public Ganho(Guid id, Guid idCliente, string nomeGanho, decimal valor, bool recorrente, DateTime dataDoGanho)
        {
            ValidaGanho(id,idCliente,nomeGanho,valor,dataDoGanho);
            
            Id = id;
            IdCliente = idCliente;
            NomeGanho = nomeGanho;
            Valor = valor;
            Recorrente = recorrente;
            DataDoGanho = dataDoGanho;
        }
        
        public Ganho(Guid id, Guid idCliente, string motivoExclusao)
        {
            ValidaExclusaoGanho(id,idCliente,motivoExclusao);
            
            Id = id;
            IdCliente = idCliente;
            MotivoExclusao = motivoExclusao;
        }

        private static void ValidaCadastroGanho(Guid idCliente, string nomeGanho, decimal valor, DateTime dataDoGanho)
        {
            if (Guid.Empty.Equals(idCliente)) 
                throw new IdException();
            
            if (string.IsNullOrWhiteSpace(nomeGanho)) 
                throw new NomeGanhoException();
            
            if (valor is <= decimal.MinValue or >= decimal.MaxValue or <= decimal.Zero)
                throw new ValorGanhoException();

            if (dataDoGanho < DateTime.MinValue.AddYears(1949) || dataDoGanho > DateTime.MaxValue.AddYears(-7899))
                throw new DataGanhoException();
        }
        
        private static void ValidaGanho(Guid id,Guid idCliente, string nomeGanho, decimal valor, DateTime dataDoGanho)
        {
            if (Guid.Empty.Equals(id)) 
                throw new IdException();
            
            if (Guid.Empty.Equals(idCliente)) 
                throw new IdException();
            
            if (string.IsNullOrWhiteSpace(nomeGanho)) 
                throw new NomeGanhoException();
            
            if (valor is <= decimal.MinValue or >= decimal.MaxValue or <= decimal.Zero)
                throw new ValorGanhoException();

            if (dataDoGanho <= DateTime.MinValue || dataDoGanho >= DateTime.MaxValue)
                throw new DataGanhoException();
        }
        
        private static void ValidaExclusaoGanho(Guid id, Guid idCliente,string motivoExclusao)
        {
            if (Guid.Empty.Equals(id)) 
                throw new IdException();
            
            if (Guid.Empty.Equals(idCliente)) 
                throw new IdException();
            
            if (string.IsNullOrWhiteSpace(motivoExclusao)) 
                throw new MotivoExclusaoException();
        }
    }
}