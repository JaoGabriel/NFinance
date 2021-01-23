using System;
using NFinance.Model.ClientesViewModel;

namespace NFinance.Model.InvestimentosViewModel
{
    public class InvestimentoViewModel
    {
        public Guid Id { get; set; }
        
        public ClienteViewModel Cliente { get; set; }
        
        public string Nome { get; set; }
        
        public decimal Valor { get; set; }
        
        public DateTime DataAplicacao { get; set; }
    }
}