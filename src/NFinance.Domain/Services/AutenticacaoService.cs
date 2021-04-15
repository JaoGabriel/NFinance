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
            var redis = new LoginViewModel.Response();
            using (var redisClient = new RedisClient(Configuration.GetConnectionString("Redis")))
            {
                redis = redisClient.Get<LoginViewModel.Response>(request.IdSessao.ToString());
            }

            var cliente = await _clienteService.ConsultarCliente(redis.IdCliente);
            //Criar um viewModel para cadastrar logout token na base, remover alteracoes no atualizar cliente viewmodel
            var clienteAtt = new AtualizarClienteViewModel.Request(cliente, redis.Token);
            var atualizado = await _clienteService.AtualizarCliente(redis.IdCliente,clienteAtt);

            //Criar um servico para cadastrar logou token no banco de dados

            if(atualizado != null)
            {
                var response = new LogoutViewModel.Response("Deslogado com sucesso!");
                return response;
            }

            return null;
        }
    } 
}