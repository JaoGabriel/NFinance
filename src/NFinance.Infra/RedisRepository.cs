using System;
using Microsoft.Extensions.Configuration;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.ViewModel.AutenticacaoViewModel;
using ServiceStack.Redis;

namespace NFinance.Domain.Services
{
    public class RedisRepository : IRedisRepository
    {
        private readonly RedisClient _cliente;

        public RedisRepository(IConfiguration Configuration)
        {
            _cliente = new RedisClient(Configuration.GetConnectionString("Redis"));
        }

        public LoginViewModel.Response RetornaValorPorChave(string chave)
        {
            var retorno = _cliente.Get<LoginViewModel.Response>(chave);
            return retorno;
        }

        public LoginViewModel.Response IncluiValorCache(LoginViewModel.Response response)
        {
            var redis = _cliente.Set<LoginViewModel.Response>(response.IdCliente.ToString(), response);
            
            if(redis)
                return response;
            else
                throw new Exception("Aconteceu um erro! Tente novamente em instantes");
            
        }

        public bool RemoverValorCache(string chave)
        {
            var retorno = _cliente.Remove(chave);
            return retorno;
        }
    } 
}