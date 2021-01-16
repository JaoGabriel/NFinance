using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IClienteService
    {
        Task<List<Cliente>> ListarClientes();
        Task<Cliente> ConsultarCliente(int id);
        Task<Cliente> CadastrarCliente(Cliente cliente);
        Task<Cliente> AtualiazarCliente(int id, Cliente cliente);
    }
}
