using Microsoft.Extensions.Configuration;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.ViewModel.AutenticacaoViewModel;
using NFinance.Domain.ViewModel.ClientesViewModel;
using ServiceStack.Redis;
using System;
using System.Threading.Tasks;

namespace NFinance.Domain.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly IClienteService _clienteService;
        private readonly IConfiguration Configuration;

        public AutenticacaoService(IClienteService clienteService, IConfiguration configuration)
        {
            _clienteService = clienteService;
            Configuration = configuration;
        }

        public async Task<LoginViewModel.Response> RealizarLogin(LoginViewModel request)
        {
            var usuarioAutenticacao = await _clienteService.ConsultarCredenciaisLogin(request);

            if (usuarioAutenticacao.Autenticado)
            {
                using (var redisClient = new RedisClient(Configuration.GetConnectionString("Redis")))
                {
                    redisClient.Set<LoginViewModel.Response>(usuarioAutenticacao.IdSessao.ToString(), usuarioAutenticacao);
                }
            }

            return usuarioAutenticacao;
        }

        public async Task<LogoutViewModel.Response> RealizarLogut(LogoutViewModel request)
        {
            var response = new LogoutViewModel.Response();
            using (var redisClient = new RedisClient(Configuration.GetConnectionString("Redis")))
            {
                var redis = redisClient.Get<LoginViewModel.Response>(request.IdSessao.ToString());
                var cliente = await _clienteService.ConsultarCliente(redis.IdCliente);
                response = await _clienteService.CadastrarLogoutToken(cliente, redis.Token);
                redisClient.Remove(request.IdSessao.ToString());
            }
            
            return response;
        }
    } 
}