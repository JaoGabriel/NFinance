using System;
using NFinance.Domain;
using NFinance.Application.ViewModel.ClientesViewModel;

namespace NFinance.Application.ViewModel.InvestimentosViewModel
{
    public class InvestimentoViewModel
    {
        public Guid Id { get; set; }
        
        public ClienteViewModel.SimpleResponse Cliente { get; set; }
        
        public string NomeInvestimento { get; set; }
        
        public decimal Valor { get; set; }
        
        public DateTime DataAplicacao { get; set; }

        public InvestimentoViewModel() { }

        public class Response
        {
            public Guid Id { get; set; }

            public Guid IdCliente { get; set; }

            public string NomeInvestimento { get; set; }

            public decimal Valor { get; set; }

            public DateTime DataAplicacao { get; set; }

            public Response() { }

            public Response(Investimento investimentos)
            {
                Id = investimentos.Id;
                IdCliente =  investimentos.IdCliente;
                NomeInvestimento = investimentos.NomeInvestimento;
                Valor = investimentos.Valor;
                DataAplicacao = investimentos.DataAplicacao;
            }
        }
    }
}