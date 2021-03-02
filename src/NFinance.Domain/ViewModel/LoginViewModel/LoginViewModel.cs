using System;

namespace NFinance.Domain.ViewModel.LoginViewModel
{
    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Senha { get; set; }

        public class Response
        {
            public Guid IdSessao { get; set; }

            public string Nome { get; set; }

            public string Token { get; set; }

            public Response() { }

            public Response(Cliente cliente, string token)
            {
                IdSessao = Guid.NewGuid();
                Nome = cliente.Nome;
                Token = token;
            }
        }
    }
}
