using System;

namespace NFinance.Model.ResgatesViewModel
{
    public class RealizarResgateViewModel
    {
        public class Request
        {
            public Guid IdCliente { get; set; }

            public Guid IdInvestimento { get; set; }
        
            public decimal Valor { get; set; }
        
            public string MotivoResgate { get; set; }
        
            public DateTime DataResgate { get; set; }
        }
        
        public class Response : ResgateViewModel { };
    }
}