using System;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IClienteService
    {
        Task<Cliente> ConsultarCliente(Guid id);
        Task<Cliente> CadastrarCliente(Cliente clienteRequest);
        Task<Cliente> AtualizarCliente(Guid id, Cliente clienteRequest);
        Task<Cliente> ConsultarCredenciaisLogin(string email, string senha);
        Task<Cliente> CadastrarLogoutToken(Cliente request,string token);
    }
}
