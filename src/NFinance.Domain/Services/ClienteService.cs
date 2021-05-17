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

        public async Task<Cliente> AtualizarCliente(Cliente clienteRequest)
        {
            if (Guid.Empty.Equals(clienteRequest.Id)) throw new IdException("Id cliente invalido");
            if (string.IsNullOrWhiteSpace(clienteRequest.Nome)) throw new NomeClienteException();
            if (string.IsNullOrWhiteSpace(clienteRequest.CPF)) throw new CpfClienteException();
            if (string.IsNullOrWhiteSpace(clienteRequest.Email)) throw new EmailClienteException();
            if (string.IsNullOrWhiteSpace(clienteRequest.Senha)) throw new SenhaClienteException();

            var resposta = await _clienteRepository.AtualizarCliente(clienteRequest);
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
            if (request == null) throw new LogoutTokenException("Deve conter cliente para cadastro de logout token!");
            if (string.IsNullOrWhiteSpace(token)) throw new LogoutTokenException("Deve conter token para cadastro!");

            var cliente = await _clienteRepository.CadastrarLogoutToken(request, token);

            if (cliente != null)
                return cliente;
            else
                return null;
            
        }

        public async Task<Cliente> ConsultarCliente(Guid id)
        {
            if (Guid.Empty.Equals(id)) throw new IdException("Id cliente invalido");

            return await _clienteRepository.ConsultarCliente(id);
        }

        public async Task<Cliente> ConsultarCredenciaisLogin(string email,string senha)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha)) throw new LoginException("Login ou senha inválidos!");

            var cliente = await _clienteRepository.ConsultarCredenciaisLogin(email, senha);

            if (cliente != null)
                return cliente;
            else
                throw new LoginException("Cliente nao encontrado!");

        }


    }
}
