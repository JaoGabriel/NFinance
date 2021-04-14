using System;

namespace NFinance.Domain.ViewModel.AutenticacaoViewModel
{
    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Senha { get; set; }

        public class Response
        {
            public Guid IdSessao { get; set; }

            public Guid IdCliente { get; set; }

            public bool Autenticado { get; set; }

            public string Nome { get; set; }

            public string Token { get; set; }

            public ErroViewModel Erro { get; set; }

            public Response() { }

            public Response(Cliente cliente, string token)
            {
                IdSessao = Guid.NewGuid();
                IdCliente = cliente.Id;
                Nome = cliente.Nome;
                Token = token;
            }

            public Response(Cliente cliente, string token,bool autenticacao)
            {
                IdSessao = Guid.NewGuid();
                IdCliente = cliente.Id;
                Nome = cliente.Nome;
                Token = token;
                Autenticado = autenticacao;
            }

            public Response(ErroViewModel erro,bool estado,Cliente cliente)
            {
                IdSessao = Guid.Empty;
                IdCliente = cliente.Id;
                Nome = cliente.Nome;
                Token = null;
                Autenticado = estado;
                Erro = erro;
            }
        }
    }
}
