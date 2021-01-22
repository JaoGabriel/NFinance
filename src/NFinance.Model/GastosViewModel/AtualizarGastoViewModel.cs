using System;

namespace NFinance.Model.GastosViewModel
{
    public class AtualizarGastoViewModel
    {
        public class Request
        {
            public Guid Cliente { get; set; }
        
            public string Nome { get; set; }

            public decimal ValorTotal { get; set; }
        
            public int QuantidadeParcelas { get; set; }
        
            public DateTime DataDoGasto { get; set; }
        }
        
        public class Response : GastoViewModel { };
    }
}