using System;
using NFinance.Domain;
using NFinance.Application.ViewModel.InvestimentosViewModel;

namespace NFinance.Application.ViewModel.ResgatesViewModel
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
    }
}