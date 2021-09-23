using System;
using NFinance.Domain;
using NFinance.Domain.Identidade;

namespace NFinance.Application.ViewModel.AutenticacaoViewModel
{
    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Senha { get; set; }

        public class Response
        {
            public Guid IdCliente { get; set; }

            public bool Autenticado { get; set; }

            public string Nome { get; set; }

            public string Token { get; set; }

            public ErroViewModel Erro { get; set; }

            public Response() { }

            public Response(Usuario usuario, string token)
            {
                IdCliente = usuario.Id;
                Nome = usuario.UserName;
                Token = token;
            }

            public Response(ErroViewModel erro,bool estado,Cliente cliente)
            {
                IdCliente = cliente.Id;
                Nome = cliente.Nome;
                Token = null;
                Autenticado = estado;
                Erro = erro;
            }
        }
    }
}
