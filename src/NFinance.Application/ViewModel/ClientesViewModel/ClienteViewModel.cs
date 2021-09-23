using System;
using NFinance.Domain;

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
            Cpf = cliente.Cpf;
            Email = cliente.Email;
        }

        public class Response
        {
            public Guid Id { get; set; }

            public string Nome { get; set; }
            
            public string Cpf { get; set; }
        
            public string Email { get; set; }

            public Response() { }

            public Response(Cliente cliente)
            {
                Id = cliente.Id;
                Nome = cliente.Nome;
                Cpf = cliente.Cpf;
                Email = cliente.Email;
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
