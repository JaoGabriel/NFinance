using System;
using System.Threading.Tasks;
using NFinance.Domain.Services;
using NFinance.Application.Interfaces;
using NFinance.Domain.Interfaces.Services;
using NFinance.Application.ViewModel.AutenticacaoViewModel;

namespace NFinance.Application
{
    public class AutenticacaoApp : IAutenticacaoApp
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public AutenticacaoApp(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        public async Task<LoginViewModel.Response> EfetuarLogin(LoginViewModel login)
        {
            var cliente = await _autenticacaoService.RealizarLogin(login.Email,login.Senha);
            var token = TokenService.GerarToken(cliente);

            return new LoginViewModel.Response(cliente,token);
        }

        public async Task<LogoutViewModel.Response> EfetuarLogoff(LogoutViewModel logout)
        {
            try
            {
                await _autenticacaoService.RealizarLogut(logout.IdCliente);
                
                return new LogoutViewModel.Response("Logout realizado com sucesso!", true);
            }
            catch (Exception)
            {
                return new LogoutViewModel.Response("Ocorreu um erro, tente novamente em instantes", false);
            }
        }

        public async Task<bool> ValidaTokenRequest(string authorization)
        {
            return await _autenticacaoService.ValidaTokenRequest(authorization);
        }
    }
}
