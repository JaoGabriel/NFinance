using System;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.ViewModel.AutenticacaoViewModel;

namespace NFinance.Domain.Interfaces.Services
{
    public class RedisService : IRedisService
    {
        private readonly IRedisRepository _redisRepository;
        public RedisService(IRedisRepository redisRepository)
        {
            _redisRepository = redisRepository;
        }

        //TODO:
        //Fazer uma valiacao em todas as chamadas para validar se o bearer token utilizado é o mesmo que o da blacklist

        public LoginViewModel.Response IncluiValorCache(LoginViewModel.Response response)
        {
            return _redisRepository.IncluiValorCache(response);
        }

        public bool RemoverValorCache(string chave)
        {
            if(chave == Guid.Empty.ToString()) throw new ArgumentNullException("IdCliente inválido");
            
            return _redisRepository.RemoverValorCache(chave);
        }

        public LoginViewModel.Response RetornaValorPorChave(string chave)
        {
            if(chave == Guid.Empty.ToString()) throw new ArgumentNullException("IdCliente inválido");

            return _redisRepository.RetornaValorPorChave(chave);
        }
    }
}
