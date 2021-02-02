using System;

namespace NFinance.Domain.ViewModel.ClientesViewModel
{
    public class ClienteViewModel
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Cpf { get; set; }
        
        public string Email { get; set; }

        public ClienteViewModel() { }

        public ClienteViewModel(Cliente cliente)
        {
            Id = cliente.Id;
            Nome = cliente.Nome;
            Cpf = cliente.CPF;
            Email = cliente.Email;
        }

        public class Response
        {
            public Guid Id { get; set; }
            public string Nome { get; set; }
            
            public string Cpf { get; set; }
        
            public string Email { get; set; }
        }
    }
}
