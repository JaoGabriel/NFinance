using System;
using NFinance.Domain;
using NFinance.WebApi.ViewModel.ClientesViewModel;

namespace NFinance.WebApi.ViewModel.GastosViewModel
{
    public class GastoViewModel
    {
        public Guid Id { get; set; }
        
        public ClienteViewModel.SimpleResponse Cliente { get; set; }
        
        public string NomeGasto { get; set; }

        public decimal Valor { get; set; }
        
        public int QuantidadeParcelas { get; set; }
        
        public DateTime DataDoGasto { get; set; }

        public GastoViewModel() { }

        public GastoViewModel(Gasto gastos)
        {
            Id = gastos.Id;
            NomeGasto = gastos.NomeGasto;
            Valor = gastos.Valor;
            QuantidadeParcelas = gastos.QuantidadeParcelas;
            DataDoGasto = gastos.DataDoGasto;
            Cliente.Id = gastos.IdCliente;
        }

        public class Response
        {
            public Guid Id { get; set; }

            public Guid IdCliente { get; set; }

            public string NomeGasto { get; set; }

            public decimal Valor { get; set; }

            public int QuantidadeParcelas { get; set; }

            public DateTime DataDoGasto { get; set; }

            public Response() { }

            public Response(Gasto gastos)
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
}