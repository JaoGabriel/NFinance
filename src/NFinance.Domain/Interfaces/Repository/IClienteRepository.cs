using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Repository
{
    public interface IClienteRepository : IDisposable
    {
        Task<List<Cliente>> ListarClientes();
        Task<Cliente> ConsultarCliente(int id);
        Task<Cliente> CadastrarCliente(Cliente cliente);
        Task<Cliente> AtualiazarCliente(int id, Cliente cliente);
    }
}
