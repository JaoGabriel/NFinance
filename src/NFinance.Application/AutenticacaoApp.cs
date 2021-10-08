using System;
using System.Threading.Tasks;
using NFinance.Application.Exceptions;
using NFinance.Application.Interfaces;
using NFinance.Application.ViewModel.AutenticacaoViewModel;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Infra.Token;

namespace NFinance.Application
{
    public class AutenticacaoApp : IAutenticacaoApp
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;

        public AutenticacaoApp(IUsuarioRepository usuarioRepository, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        public async Task<LoginViewModel.Response> EfetuarLogin(LoginViewModel login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha)) throw new AutenticacaoException("Email ou senha invalida");

            var usuario = await _usuarioRepository.Conectar(login.Email, login.Senha);

            if (usuario is null) throw new AutenticacaoException("Ocorreu um erro ao efetuar o login.");

            var token = _tokenService.GerarToken(usuario);
            
            return new LoginViewModel.Response(usuario,token);
        }

        public async Task<LogoutViewModel.Response> EfetuarLogoff(LogoutViewModel logout)
        {
            if (Guid.Empty.Equals(logout.IdCliente)) throw new AutenticacaoException("Id invalido");

            await _usuarioRepository.Desconectar();
            
            return new LogoutViewModel.Response("Logout realizado com sucesso!", true);
        }
    }
}
