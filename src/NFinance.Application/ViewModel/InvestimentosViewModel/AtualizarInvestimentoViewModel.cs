﻿using System;
using NFinance.Domain;

namespace NFinance.Application.ViewModel.InvestimentosViewModel
{
    public class AtualizarInvestimentoViewModel
    {
        public class Request
        {
            public Guid IdCliente { get; set; }
        
            public string NomeInvestimento { get; set; }
        
            public decimal Valor { get; set; }
        
            public DateTimeOffset DataAplicacao { get; set; }
        }

        public class Response : InvestimentoViewModel
        {
            public Response(Investimento investimentos) : base(investimentos)
            {
            }
        }
    }
}