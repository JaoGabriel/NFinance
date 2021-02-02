using System;
using NFinance.Domain;
using NFinance.Domain.ViewModel.ClientesViewModel;

namespace NFinance.Model.GastosViewModel
{
    public class GastoViewModel
    {
        public Guid Id { get; set; }
        
        public ClienteViewModel.Response Cliente { get; set; }
        
        public string NomeGasto { get; set; }

        public decimal ValorTotal { get; set; }
        
        public int QuantidadeParcelas { get; set; }
        
        public DateTime DataDoGasto { get; set; }

        public GastoViewModel() { }

        public GastoViewModel(Gastos gastos)
        {
            Id = gastos.Id;
            NomeGasto = gastos.NomeGasto;
            ValorTotal = gastos.ValorTotal;
            QuantidadeParcelas = gastos.QuantidadeParcelas;
            DataDoGasto = gastos.DataDoGasto;
            Cliente.Id = gastos.IdCliente;
        }

        public class Response
        {
            public Guid Id { get; set; }

            public Guid IdCliente { get; set; }

            public string NomeGasto { get; set; }

            public decimal ValorTotal { get; set; }

            public int QuantidadeParcelas { get; set; }

            public DateTime DataDoGasto { get; set; }

            public Response() { }

            public Response(Gastos gastos)
            {
                Id = gastos.Id;
                NomeGasto = gastos.NomeGasto;
                ValorTotal = gastos.ValorTotal;
                QuantidadeParcelas = gastos.QuantidadeParcelas;
                DataDoGasto = gastos.DataDoGasto;
                IdCliente = gastos.IdCliente;
            }
        }
    }
}