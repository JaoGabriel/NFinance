using NFinance.Domain.ViewModel.AutenticacaoViewModel;

namespace NFinance.Domain.Interfaces.Repository
{
    public interface IRedisRepository
    {
        public LoginViewModel.Response RetornaValorPorChave(string chave);

        public LoginViewModel.Response IncluiValorCache(LoginViewModel.Response response);

        public bool RemoverValorCache(string chave);
    }
}
