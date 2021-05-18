using NFinance.Domain;

namespace NFinance.Application.ViewModel.ClientesViewModel
{
    public class ConsultarClienteViewModel
    {
        public class Response : ClienteViewModel
        {
            public Response(Cliente cliente) : base(cliente)
            {
            }
        }
    }
}
