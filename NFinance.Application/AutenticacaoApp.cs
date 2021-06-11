using System;
using System.Linq;
using System.Threading.Tasks;
using NFinance.Domain.Exceptions;
using NFinance.Application.Interfaces;
using NFinance.Domain.Exceptions.Autenticacao;
using NFinance.Application.ViewModel.AutenticacaoViewModel;

namespace NFinance.Application
{
    public class AutenticacaoApp : IAutenticacaoApp
    {
        private readonly IClienteApp _clienteApp;
        private readonly IRedisApp _redisApp;

        public AutenticacaoApp(IClienteApp clienteApp)
        {
            _clienteApp = clienteApp;
        }

        public async Task<LoginViewModel.Response> EfetuarLogin(LoginViewModel login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha)) throw new LoginException("Email ou senha invalida");
            
            var usuarioAutenticacao = await _clienteApp.ConsultarCredenciaisLogin(login.Email,login.Senha);

            if (usuarioAutenticacao == null) throw new LoginException("Ocorreu um erro, tente novamente!");

            var token = TokenApp.GerarToken(usuarioAutenticacao);

            _redisApp.IncluiValorCache(usuarioAutenticacao);

            return new LoginViewModel.Response(usuarioAutenticacao, token);
        }

        public async Task<LogoutViewModel.Response> EfetuarLogoff(LogoutViewModel logout)
        {
            if (Guid.Empty.Equals(logout.IdCliente)) throw new IdException("Id invalido");

            var valorRedis = _redisApp.RetornaValorPorChave(logout.IdCliente.ToString());
            await _clienteApp.CadastrarLogoutToken(valorRedis);
            var clienteExcluido = _redisApp.RemoverValorCache(logout.IdCliente.ToString());

            if (clienteExcluido)
                return new LogoutViewModel.Response("Logout realizado com sucesso!", true);
            else
                return new LogoutViewModel.Response("Ocorreu um erro, tente novamente em instantes", false);
        }
        public async Task<bool> ValidaTokenRequest(string authorization)
        {
            var listaToken = TokenApp.LerToken(authorization);
            var redisToken = _redisApp.RetornaValorPorChave(listaToken.FirstOrDefault()).LogoutToken;
            var cliente = await _clienteApp.ConsultaCliente(Guid.Parse(listaToken.FirstOrDefault()));

            if (cliente.LogoutToken == listaToken.FirstOrDefault(token => token == authorization[7..]) || authorization[7..] != redisToken)
                throw new TokenException();
            else
                return true;
        }
    }
}
