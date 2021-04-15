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
            public string Message { get; set; }

            public Response(string message)
            {
                Message = message;
            }
        }
    }
}
