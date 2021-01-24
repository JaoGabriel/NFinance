using System;
using NFinance.Domain;
using NFinance.Model.ClientesViewModel;

namespace NFinance.Model.GastosViewModel
{
    public class GastoViewModel
    {
        public Guid Id { get; set; }
        
        public ClienteViewModel.Response Cliente { get; set; }
        
        public string Nome { get; set; }

        public decimal ValorTotal { get; set; }
        
        public int QuantidadeParcelas { get; set; }
        
        public DateTime DataDoGasto { get; set; }

        public GastoViewModel() { }

        public GastoViewModel(Gastos gastos)
        {
            Id = gastos.Id;
            Nome = gastos.Nome;
            ValorTotal = gastos.ValorTotal;
            QuantidadeParcelas = gastos.QuantidadeParcelas;
            DataDoGasto = gastos.DataDoGasto;
        }
    }
}