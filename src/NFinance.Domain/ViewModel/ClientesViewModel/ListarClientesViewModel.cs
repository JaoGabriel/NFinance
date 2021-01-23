using NFinance.Domain;
using System.Collections.Generic;

namespace NFinance.Model.ClientesViewModel
{
    public class ListarClientesViewModel
    {
        public class Response : List<ClienteViewModel>
        {
            public Response(List<Cliente> cliente)
            {
                foreach (var item in cliente)
                {
                    var clienteVm = new ClienteViewModel(item);
                    this.Add(clienteVm);
                }   
            }
        }
    }
}
