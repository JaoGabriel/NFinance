using NFinance.Domain;
using NFinance.Infra.Identidade;

namespace NFinance.Application.Interfaces
{
    public interface IRedisApp
    {
        public bool IncluiValorCache(Usuario usuario);
        public bool RemoverValorCache(string chave);
        public Cliente RetornaValorPorChave(string chave);
    }
}
