using System;

namespace NFinance.Application.ViewModel.ClientesViewModel
{
    public class ClienteViewModel
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Cpf { get; set; }
        
        public string Email { get; set; }

        public string Senha { get; set; }

        public ClienteViewModel() { }

        public ClienteViewModel(Cliente cliente)
        {
            Id = cliente.Id;
            Nome = cliente.Nome;
            Cpf = cliente.CPF;
            Email = cliente.Email;
            Senha = cliente.Senha;
        }

        public class Response
        {
            public Guid Id { get; set; }

            public string Nome { get; set; }
            
            public string Cpf { get; set; }
        
            public string Email { get; set; }

            public string BlackListToken { get; set; }

            public Response() { }

            public Response(Cliente cliente)
            {
                Id = cliente.Id;
                Nome = cliente.Nome;
                Cpf = cliente.CPF;
                Email = cliente.Email;
                BlackListToken = cliente.LogoutToken;
            }
        }

        public class SimpleResponse
        {
            public Guid Id { get; set; }

            public string Nome { get; set; }

            public SimpleResponse() { }

            public SimpleResponse(Cliente cliente)
            {
                Id = cliente.Id;
                Nome = cliente.Nome;
            }
        }
    }
}
