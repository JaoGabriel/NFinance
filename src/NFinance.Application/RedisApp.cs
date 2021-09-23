using System;
using NFinance.Domain;
using NFinance.Application.Interfaces;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Exceptions.Autenticacao;
using NFinance.Infra.Identidade;

namespace NFinance.Application
{
    public class RedisApp : IRedisApp
    {
        private readonly IRedisRepository _redisRepository;
        
        public RedisApp(IRedisRepository redisRepository)
        {
            _redisRepository = redisRepository;
        }

        public bool IncluiValorCache(Usuario usuario)
        {
            if (usuario == null) throw new Exception($"Nao é possivel incluir {usuario.IdUsuario}");

            var resultadoInclusao = _redisRepository.IncluiValorCache(usuario);

            if (!resultadoInclusao)
                throw new LoginException("Falha ao efetuar o login, tente novamente em instantes!");

            return true;
        }

        public bool RemoverValorCache(string chave)
        {
            if(chave == Guid.Empty.ToString()) throw new ArgumentNullException("IdCliente inválido");
            
            return _redisRepository.RemoverValorCache(chave);
        }

        public Cliente RetornaValorPorChave(string chave)
        {
            if(chave == Guid.Empty.ToString()) throw new ArgumentNullException("IdCliente inválido");

            return _redisRepository.RetornaValorPorChave(chave);
        }
    }
}
