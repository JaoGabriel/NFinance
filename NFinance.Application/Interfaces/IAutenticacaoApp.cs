using System.Threading.Tasks;
using NFinance.Application.ViewModel.AutenticacaoViewModel;

namespace NFinance.Application.Interfaces
{
    public interface IAutenticacaoApp
    {
        Task<LoginViewModel.Response> EfetuarLogin(LoginViewModel login);
        Task<LogoutViewModel.Response> EfetuarLogoff(LogoutViewModel logout);
        Task<bool> ValidaTokenRequest(string authorization);
    }
}
