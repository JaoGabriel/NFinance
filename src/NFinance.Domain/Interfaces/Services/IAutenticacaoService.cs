using System;
using System.Threading.Tasks;
using NFinance.Domain.ViewModel.AutenticacaoViewModel;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IAutenticacaoService
    {
        Task<LoginViewModel.Response> RealizarLogin(LoginViewModel request);
        Task<LogoutViewModel.Response> RealizarLogut(Guid id);
        Task<bool> ValidaTokenRequest(string authorization);
    }
}