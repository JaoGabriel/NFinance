using System;
using System.Threading.Tasks;
using NFinance.Domain.Exceptions;
using NFinance.Application.Interfaces;
using NFinance.Domain.Exceptions.Autenticacao;
using NFinance.Application.ViewModel.AutenticacaoViewModel;
using NFinance.Domain.Repository;

namespace NFinance.Application
{
    public class AutenticacaoApp : IAutenticacaoApp
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AutenticacaoApp(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<LoginViewModel.Response> EfetuarLogin(LoginViewModel login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha)) throw new LoginException("Email ou senha invalida");

            var usuario = await _usuarioRepository.Conectar(login.Email, login.Senha);

            var token = TokenApp.GerarToken(usuario);

            return new LoginViewModel.Response(usuario, token);
        }

        public async Task<LogoutViewModel.Response> EfetuarLogoff(LogoutViewModel logout)
        {
            if (Guid.Empty.Equals(logout.IdCliente)) throw new IdException("Id invalido");

            await _usuarioRepository.Desconectar();
            
            return new LogoutViewModel.Response("Logout realizado com sucesso!", true);
        }
    }
}
