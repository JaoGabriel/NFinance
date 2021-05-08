using System;

namespace NFinance.WebApi.ViewModel.AutenticacaoViewModel
{
    public class LogoutViewModel
    {
        public Guid IdCliente { get; set; }

        public LogoutViewModel(Guid idCliente)
        {
            IdCliente = idCliente;
        }

        public class Response
        {
            public bool Deslogado { get; set; }
            public string Message { get; set; }

            public Response() { }

            public Response(string message,bool estado)
            {
                Message = message;
                Deslogado = estado;
            }

            public Response(string message)
            {
                Message = message;
            }
        }
    }
}
