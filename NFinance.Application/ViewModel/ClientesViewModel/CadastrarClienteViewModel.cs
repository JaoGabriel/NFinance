using NFinance.Domain;

namespace NFinance.Application.ViewModel.ClientesViewModel
{
    public class CadastrarClienteViewModel
    {
        public class Request : ClienteViewModel
        {
            public Request(Cliente cliente) : base(cliente) { }
        }

        public class Response : ClienteViewModel
        {
            public Response(Cliente cliente) : base(cliente) { }
        }
    }
}
