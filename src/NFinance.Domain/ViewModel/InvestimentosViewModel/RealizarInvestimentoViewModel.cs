﻿using System;

namespace NFinance.Model.InvestimentosViewModel
{
    public class RealizarInvestimentoViewModel
    {
        public class Request
        {
            public Guid IdCliente { get; set; }

            public string Nome { get; set; }
        
            public decimal Valor { get; set; }
        
            public DateTime DataAplicacao { get; set; }
        }
        
        public class Response : InvestimentoViewModel { };
    }
}