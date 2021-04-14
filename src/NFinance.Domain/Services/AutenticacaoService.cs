using Microsoft.Extensions.Configuration;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.ViewModel.AutenticacaoViewModel;
using ServiceStack.Redis;
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

            if (usuarioAutenticacao.Autenticado == true)
            {
                using (var redisClient = new RedisClient(Configuration.GetConnectionString("Redis")))
                {
                    redisClient.Set<LoginViewModel.Response>(usuarioAutenticacao.IdSessao.ToString(), usuarioAutenticacao);
                }
            }

            return usuarioAutenticacao;
        }

        public Task<LogoutViewModel> RealizarLogut()
        {
            return null;
        }
    } 
}