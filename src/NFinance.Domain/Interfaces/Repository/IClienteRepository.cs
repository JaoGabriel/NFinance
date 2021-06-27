using System;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Repository
{
    public interface IClienteRepository
    {
        Task<Cliente> ConsultarCliente(Guid id);
        
        Task<Cliente> CadastrarCliente(Cliente cliente);
        
        Task<Cliente> AtualizarCliente(Cliente cliente);
        
        Task CadastrarLogoutToken(Guid idCliente, string token);
    }
}
