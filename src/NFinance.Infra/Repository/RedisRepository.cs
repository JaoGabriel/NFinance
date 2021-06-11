using NFinance.Domain;
using ServiceStack.Redis;
using Microsoft.Extensions.Configuration;
using NFinance.Domain.Interfaces.Repository;

namespace NFinance.Infra.Repository
{
    public class RedisRepository : IRedisRepository
    {
        private readonly RedisClient _cliente;

        public RedisRepository(IConfiguration Configuration)
        {
            _cliente = new RedisClient(Configuration.GetConnectionString("Redis"));
        }

        public Cliente RetornaValorPorChave(string chave)
        {
            var retorno = _cliente.Get<Cliente>(chave);
            return retorno;
        }

        public bool IncluiValorCache(Cliente cliente)
        {
            var redis = _cliente.Set(cliente.Id.ToString(), cliente);
            
            if(redis)
                return true;
            else
                return false;

        }

        public bool RemoverValorCache(string chave)
        {
            var retorno = _cliente.Remove(chave);
            return retorno;
        }
    } 
}