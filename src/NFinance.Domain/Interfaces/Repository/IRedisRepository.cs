namespace NFinance.Domain.Interfaces.Repository
{
    public interface IRedisRepository
    {
        public Cliente RetornaValorPorChave(string chave);

        public bool IncluiValorCache(Cliente cliente);

        public bool RemoverValorCache(string chave);
    }
}
