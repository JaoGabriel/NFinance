using NFinance.Domain;

namespace NFinance.Application.ViewModel.ClientesViewModel
{
    public class CadastrarClienteViewModel
    {
        public class Request
        {
            public string Nome { get; set; }

            public string Cpf { get; set; }
            
            public string Email { get; set; }

            public string Senha { get; set; }
        }

        public class Response
        {
            public string Nome { get; set; }

            public string Cpf { get; set; }

            public string Email { get; set; }

            public string Senha { get; set; }

            public Response() { }

            public Response(Cliente cliente)
            {
                Nome = cliente.Nome;
                Cpf = cliente.CPF;
                Email = cliente.Email;
                Senha = cliente.Senha;
            }
        }
    }
}
