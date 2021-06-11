using NFinance.Domain;

namespace NFinance.Application.Interfaces
{
    public interface IRedisApp
    {
        public bool IncluiValorCache(Cliente cliente);
        public bool RemoverValorCache(string chave);
        public Cliente RetornaValorPorChave(string chave);
    }
}
