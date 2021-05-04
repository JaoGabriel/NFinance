using NFinance.Domain.Exceptions.Autenticacao;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.ViewModel.AutenticacaoViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NFinance.Domain.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly IClienteService _clienteService;
        private readonly IRedisService _redis;

        public AutenticacaoService(IClienteService clienteService,IRedisService redis)
        {
            _clienteService = clienteService;
            _redis = redis;
        }

        public async Task<LoginViewModel.Response> RealizarLogin(LoginViewModel request)
        {
            var usuarioAutenticacao = await _clienteService.ConsultarCredenciaisLogin(request);
            _redis.IncluiValorCache(usuarioAutenticacao);

            return usuarioAutenticacao;
        }

        public async Task<LogoutViewModel.Response> RealizarLogut(LogoutViewModel request)
        {            
            var valorRedis = _redis.RetornaValorPorChave(request.IdCliente.ToString());
            var cliente = await _clienteService.ConsultarCliente(valorRedis.IdCliente);
            var response = await _clienteService.CadastrarLogoutToken(cliente, valorRedis.Token);
            var clienteExcluido = _redis.RemoverValorCache(request.IdCliente.ToString());
            
            if(clienteExcluido)
                return response;
            else
                throw new LogoutException("Aconteceu um erro! Tente novamente em instantes!");
        }

        public async Task<bool> ValidaTokenRequest(string authorization)
        {
            var listaToken = TokenService.LerToken(authorization);
            var redisToken = _redis.RetornaValorPorChave(listaToken.FirstOrDefault().ToString()).Token;
            var cliente = await _clienteService.ConsultarCliente(Guid.Parse(listaToken.FirstOrDefault().ToString()));

            if (cliente.BlackListToken == listaToken.FirstOrDefault(token => token == authorization.Substring(7)) || authorization.Substring(7) != redisToken)
                throw new TokenException();
            else
                return true;
        }
    } 
}