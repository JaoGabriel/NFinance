using System;
using NFinance.Domain;

namespace NFinance.Application.ViewModel.GastosViewModel
{
    public class GastoViewModel
    {
        public Guid Id { get; set; }
        
        public Guid IdCliente { get; set; }
        
        public string NomeGasto { get; set; }

        public decimal Valor { get; set; }
        
        public int QuantidadeParcelas { get; set; }
        
        public DateTimeOffset DataDoGasto { get; set; }

        public GastoViewModel() { }

        public GastoViewModel(Gasto gastos)
        {
            Id = gastos.Id;
            NomeGasto = gastos.NomeGasto;
            Valor = gastos.Valor;
            QuantidadeParcelas = gastos.QuantidadeParcelas;
            DataDoGasto = gastos.DataDoGasto;
            IdCliente = gastos.IdCliente;
        }
    }
}