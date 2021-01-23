using System;
using NFinance.Model.ClientesViewModel;

namespace NFinance.Model.GastosViewModel
{
    public class GastoViewModel
    {
        public Guid Id { get; set; }
        
        public ClienteViewModel Cliente { get; set; }
        
        public string Nome { get; set; }

        public decimal ValorTotal { get; set; }
        
        public int QuantidadeParcelas { get; set; }
        
        public DateTime DataDoGasto { get; set; }
    }
}