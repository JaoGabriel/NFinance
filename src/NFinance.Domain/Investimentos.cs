using NFinance.Model.InvestimentosViewModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NFinance.Domain
{
    public class Investimentos
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [ForeignKey("Id")]
        [Required]
        public Guid IdCliente { get; set; }

        [Required(ErrorMessage = "Nome do Investimento deve conter no minimo 10 letras e no maximo 100")]
        [StringLength(100, MinimumLength = 10)]
        public string NomeInvestimento { get; set; }

        [Required]
        [Range(0, 999999999999, ErrorMessage = "Valor {0} deve estar entre {1} e {2}")]
        public decimal Valor { get; set; }
        
        [Required]
        [Range(typeof(DateTime),"01/01/1950","12/31/2100", ErrorMessage = "Data {0} deve estar entre {1} e {2}")]
        public DateTime DataAplicacao { get; set; }

        public Investimentos() { }

        public Investimentos(Investimentos investimentos)
        {
            Id = Guid.NewGuid();
            IdCliente = investimentos.IdCliente;
            NomeInvestimento = investimentos.NomeInvestimento;
            Valor = investimentos.Valor;
            DataAplicacao = investimentos.DataAplicacao;
        }

        public Investimentos(RealizarInvestimentoViewModel.Request request)
        {
            Id = Guid.NewGuid();
            IdCliente = request.IdCliente;
            NomeInvestimento = request.NomeInvestimento;
            Valor = request.Valor;
            DataAplicacao = request.DataAplicacao;
        }

        public Investimentos(Guid id, AtualizarInvestimentoViewModel.Request request)
        {
            Id = id;
            IdCliente = request.IdCliente;
            NomeInvestimento = request.NomeInvestimento;
            Valor = request.Valor;
            DataAplicacao = request.DataAplicacao;
        }
    }
}
