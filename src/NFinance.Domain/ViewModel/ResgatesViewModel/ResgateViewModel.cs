using System;
using NFinance.Domain;
using NFinance.ViewModel.InvestimentosViewModel;

namespace NFinance.ViewModel.ResgatesViewModel
{
    public class ResgateViewModel
    {
        public Guid Id { get; set; }

        public Guid IdCliente { get; set; }

        public InvestimentoViewModel Investimento { get; set; }
        
        public decimal Valor { get; set; }
        
        public string MotivoResgate { get; set; }
        
        public DateTime DataResgate { get; set; }

        public ResgateViewModel() { }

        public ResgateViewModel(Resgate resgate)
        {
            Id = resgate.Id;
            Valor = resgate.Valor;
            MotivoResgate = resgate.MotivoResgate;
            DataResgate = resgate.DataResgate;
            Investimento.Id = resgate.IdInvestimento;
        }

        public class Response
        {

            public Guid Id { get; set; }

            public Guid IdCliente { get; set; }

            public Guid IdInvestimento { get; set; }

            public decimal Valor { get; set; }

            public string MotivoResgate { get; set; }

            public DateTime DataResgate { get; set; }

            public Response() { }

            public Response(Resgate resgate)
            {
                Id = resgate.Id;
                IdInvestimento = resgate.IdInvestimento;
                IdCliente= resgate.IdCliente;
                Valor = resgate.Valor;
                MotivoResgate = resgate.MotivoResgate;
                DataResgate = resgate.DataResgate;
            }
        }
    }
}