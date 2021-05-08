using System;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IAutenticacaoService
    {
        Task<Cliente> RealizarLogin(string email, string senha);
        Task<Cliente> RealizarLogut(Guid id);
        Task<bool> ValidaTokenRequest(string authorization);
    }
}