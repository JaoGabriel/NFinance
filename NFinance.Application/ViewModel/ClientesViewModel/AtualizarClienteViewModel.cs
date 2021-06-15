using NFinance.Domain;

namespace NFinance.Application.ViewModel.ClientesViewModel
{
    public class AtualizarClienteViewModel
    {
        public class Request : ClienteViewModel
        {
            public Request() { }
            
            public Request(Cliente request)
            {
                Nome = request.Nome;
                Cpf = request.CPF;
                Email = request.Email;
                Senha = request.Senha;
            }
        }

        public class Response : ClienteViewModel
        {
            public Response(Cliente cliente) : base(cliente) { }
        }
    }
}
