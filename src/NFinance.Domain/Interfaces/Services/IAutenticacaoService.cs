using System.Threading.Tasks;
using NFinance.Domain.ViewModel.AutenticacaoViewModel;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IAutenticacaoService
    {
        Task<LoginViewModel.Response> RealizarLogin(LoginViewModel request);
        Task<LogoutViewModel.Response> RealizarLogut(LogoutViewModel request);
        Task<bool> ValidaTokenRequest(string authorization);
    }
}