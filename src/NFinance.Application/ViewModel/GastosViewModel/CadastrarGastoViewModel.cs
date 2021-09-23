﻿using System;
using NFinance.Domain;

namespace NFinance.Application.ViewModel.GastosViewModel
{
    public class CadastrarGastoViewModel
    {
        public class Request
        {
            public Guid IdCliente { get; set; }
        
            public string NomeGasto { get; set; }

            public decimal Valor { get; set; }
        
            public int QuantidadeParcelas { get; set; }
        
            public DateTime DataDoGasto { get; set; }
        }

        public class Response : GastoViewModel
        {
            public Response(Gasto gastos) : base(gastos) { }
        }
    }
}