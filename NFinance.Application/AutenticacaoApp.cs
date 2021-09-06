using System;
using System.Threading.Tasks;
using NFinance.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using NFinance.Application.Interfaces;
using NFinance.Domain.Exceptions.Autenticacao;
using NFinance.Application.ViewModel.AutenticacaoViewModel;
using NFinance.Domain.Identidade;

namespace NFinance.Application
{
    public class AutenticacaoApp : IAutenticacaoApp
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public AutenticacaoApp(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
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
            
            return new LogoutViewModel.Response("Logout realizado com sucesso!", true);
        }
    }
}
