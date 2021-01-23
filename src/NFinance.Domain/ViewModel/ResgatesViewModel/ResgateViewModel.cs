using System;
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
    }
}