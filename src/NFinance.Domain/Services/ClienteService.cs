using System;
using System.Threading.Tasks;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Cliente;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Exceptions.Autenticacao;

namespace NFinance.Domain.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Cliente> AtualizarCliente(Guid id, Cliente clienteRequest)
        {
            if (Guid.Empty.Equals(id)) throw new IdException("Id cliente invalido");
            if (string.IsNullOrWhiteSpace(clienteRequest.Nome)) throw new NomeClienteException();
            if (string.IsNullOrWhiteSpace(clienteRequest.CPF)) throw new CpfClienteException();
            if (string.IsNullOrWhiteSpace(clienteRequest.Email)) throw new EmailClienteException();
            if (string.IsNullOrWhiteSpace(clienteRequest.Senha)) throw new SenhaClienteException();

            var resposta = await _clienteRepository.AtualizarCliente(id, clienteRequest);
            return resposta;
        }

        public async Task<Cliente> CadastrarCliente(Cliente clienteRequest)
        {
            if (string.IsNullOrWhiteSpace(clienteRequest.Nome)) throw new NomeClienteException();
            if (string.IsNullOrWhiteSpace(clienteRequest.CPF)) throw new CpfClienteException();
            if (string.IsNullOrWhiteSpace(clienteRequest.Email)) throw new EmailClienteException();
            if (string.IsNullOrWhiteSpace(clienteRequest.Senha)) throw new SenhaClienteException();

            var resposta = await _clienteRepository.CadastrarCliente(clienteRequest);
            return resposta;
        }

        public async Task<Cliente> CadastrarLogoutToken(Cliente request, string token)
        {
            if (request == null) throw new ArgumentException("Ocorreu um erro, Tente novamente em instantes!");

            var cliente = new Cliente(request, token);
            var clienteLogoutToken = await _clienteRepository.CadastrarLogoutToken(cliente);

            if (clienteLogoutToken != null)
                return cliente;
            else
                return null;
            
        }

        public async Task<Cliente> ConsultarCliente(Guid id)
        {
            if (Guid.Empty.Equals(id)) throw new IdException("Id cliente invalido");

            return await _clienteRepository.ConsultarCliente(id);
        }

        public async Task<Cliente> ConsultarCredenciaisLogin(Cliente request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha)) throw new LoginException("Login ou senha inválidos!");

            var credenciaisCliente = await _clienteRepository.CredenciaisLogin(request.Email, request.Senha);

            if (credenciaisCliente != null)
            {
                var token = TokenService.GerarToken(credenciaisCliente);
                request.LoginToken = token;
                return request;
            }
            else
                return null;

        }


    }
}
