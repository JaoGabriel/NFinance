using System;
using NFinance.Domain;
using NFinance.Model.InvestimentosViewModel;

namespace NFinance.Model.ResgatesViewModel
{
    public class ResgateViewModel
    {
        public Guid Id { get; set; }
        
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
        }
    }
}