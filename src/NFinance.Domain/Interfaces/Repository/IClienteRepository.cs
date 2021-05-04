using System;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Repository
{
    public interface IClienteRepository : IDisposable
    {
        Task<Cliente> ConsultarCliente(Guid id);
        Task<Cliente> CadastrarCliente(Cliente cliente);
        Task<Cliente> AtualizarCliente(Guid id, Cliente cliente);
        Task<Cliente> CredenciaisLogin(string usuario,string senha);
        Task<Cliente> CadastrarLogoutToken(Cliente cliente);
    }
}
