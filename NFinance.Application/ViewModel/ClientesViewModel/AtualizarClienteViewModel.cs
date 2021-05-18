using NFinance.Domain;

namespace NFinance.Application.ViewModel.ClientesViewModel
{
    public class AtualizarClienteViewModel
    {
        public class Request : ClienteViewModel
        {
            public Request(Request response)
            {
                Nome = response.Nome;
                Cpf = response.Cpf;
                Email = response.Email;
            }
        }

        public class Response : ClienteViewModel
        {
            public Response(Cliente cliente) : base(cliente) { }
        }
    }
}
