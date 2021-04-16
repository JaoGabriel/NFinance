using System;

namespace NFinance.Domain.ViewModel.AutenticacaoViewModel
{
    public class LogoutViewModel
    {
        public Guid IdSessao { get; set; }

        public LogoutViewModel(Guid idSessao)
        {
            IdSessao = idSessao;
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
