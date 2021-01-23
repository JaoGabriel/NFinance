using System;

namespace NFinance.Model.GastosViewModel
{
    public class ExcluirGastoViewModel
    {
        public class Request
        {
            public Guid IdGasto { get; set; }
            
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