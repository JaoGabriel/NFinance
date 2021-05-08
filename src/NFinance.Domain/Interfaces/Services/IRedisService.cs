namespace NFinance.Domain.Interfaces.Services
{
    public interface IRedisService
    {
        public Cliente RetornaValorPorChave(string chave);

        public bool IncluiValorCache(Cliente cliente);

        public bool RemoverValorCache(string chave);
    }
}
