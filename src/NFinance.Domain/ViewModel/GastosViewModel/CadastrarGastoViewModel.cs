using System;

namespace NFinance.ViewModel.GastosViewModel
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
        
        public class Response : GastoViewModel { };
    }
}