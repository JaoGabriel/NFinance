using System;

namespace NFinance.WebApi.ViewModel.GastosViewModel
{
    public class ExcluirGastoViewModel
    {
        public class Request
        {
            public Guid IdGasto { get; set; }

            public Guid IdCliente { get; set; }

            public string MotivoExclusao { get; set; }
        }

        public class Response
        {
            public int StatusCode { get; set; }
            
            public string Mensagem { get; set; }
            
            public DateTime DataExclusao { get; set; }
        }
    }
}