using System;
using System.Linq;
using System.Threading.Tasks;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Exceptions.Autenticacao;

namespace NFinance.Domain.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly IRedisService _redis;
        private readonly IClienteService _clienteService;

        public AutenticacaoService(IClienteService clienteService,IRedisService redis)
        {
            _redis = redis;
            _clienteService = clienteService;
        }

        public async Task<Cliente> RealizarLogin(string email, string senha)
        {
            var usuarioAutenticacao = await _clienteService.ConsultarCredenciaisLogin(email,senha);
            _redis.IncluiValorCache(usuarioAutenticacao);

            return usuarioAutenticacao;
        }

        public async Task<Cliente> RealizarLogut(Guid id)
        {            
            var valorRedis = _redis.RetornaValorPorChave(id.ToString());
            var cliente = await _clienteService.ConsultarCliente(valorRedis.Id);
            var response = await _clienteService.CadastrarLogoutToken(cliente, valorRedis.LogoutToken);
            var clienteExcluido = _redis.RemoverValorCache(id.ToString());
            
            if(clienteExcluido)
                return response;
            else
                throw new LogoutException("Aconteceu um erro! Tente novamente em instantes!");
        }

        public async Task<bool> ValidaTokenRequest(string authorization)
        {
            var listaToken = TokenService.LerToken(authorization);
            var redisToken = _redis.RetornaValorPorChave(listaToken.FirstOrDefault().ToString()).LogoutToken;
            var cliente = await _clienteService.ConsultarCliente(Guid.Parse(listaToken.FirstOrDefault().ToString()));

            if (cliente.LogoutToken == listaToken.FirstOrDefault(token => token == authorization.Substring(7)) || authorization.Substring(7) != redisToken)
                throw new TokenException();
            else
                return true;
        }
    } 
}