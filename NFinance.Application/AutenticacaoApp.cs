using System;
using System.Threading.Tasks;
using NFinance.Infra.Identidade;
using NFinance.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using NFinance.Application.Interfaces;
using NFinance.Domain.Exceptions.Autenticacao;
using NFinance.Application.ViewModel.AutenticacaoViewModel;

namespace NFinance.Application
{
    public class AutenticacaoApp : IAutenticacaoApp
    {
        private readonly IClienteApp _clienteApp;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public AutenticacaoApp(IClienteApp clienteApp, UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            _clienteApp = clienteApp;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<LoginViewModel.Response> EfetuarLogin(LoginViewModel login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha)) throw new LoginException("Email ou senha invalida");

            var loginResponse = await _signInManager.PasswordSignInAsync(login.Email, login.Senha, false, true);

            if (!loginResponse.Succeeded)
                throw new LoginException("Usuario ou senha inválido, tente novamente");

            var usuario = await _userManager.FindByEmailAsync(login.Email);

            var token = TokenApp.GerarToken(usuario);

            return new LoginViewModel.Response(usuario, token);
        }

        public async Task<LogoutViewModel.Response> EfetuarLogoff(LogoutViewModel logout)
        {
            if (Guid.Empty.Equals(logout.IdCliente)) throw new IdException("Id invalido");

            await _signInManager.SignOutAsync();
            
            var logoutToken = _clienteApp.CadastraLogoutToken(logout).IsCompleted;

            if (logoutToken is false)
                return new LogoutViewModel.Response("Ocorreu um erro, tente novamente em instantes", false);

            return new LogoutViewModel.Response("Logout realizado com sucesso!", true);
        }
    }
}
