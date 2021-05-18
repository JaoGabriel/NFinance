using System;
using NFinance.Domain;

namespace NFinance.Application.ViewModel.InvestimentosViewModel
{
    public class InvestimentoViewModel
    {
        public Guid Id { get; set; }

        public Guid IdCliente { get; set; }

        public string NomeInvestimento { get; set; }

        public decimal Valor { get; set; }

        public DateTime DataAplicacao { get; set; }

        public InvestimentoViewModel() { }

        public InvestimentoViewModel(Investimento investimentos)
        {
            Id = investimentos.Id;
            IdCliente = investimentos.IdCliente;
            NomeInvestimento = investimentos.NomeInvestimento;
            Valor = investimentos.Valor;
            DataAplicacao = investimentos.DataAplicacao;
        }
    }
}