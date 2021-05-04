using NFinance.Domain.ViewModel.AutenticacaoViewModel;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IRedisService
    {
        public LoginViewModel.Response RetornaValorPorChave(string chave);

        public LoginViewModel.Response IncluiValorCache(LoginViewModel.Response response);

        public bool RemoverValorCache(string chave);
    }
}
