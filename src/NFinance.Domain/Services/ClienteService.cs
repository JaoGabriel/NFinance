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
            var resposta = await _clienteRepository.AtualizarCliente(clienteRequest);
            return resposta;
        }

        public async Task<Cliente> CadastrarCliente(Cliente clienteRequest)
        {
            var resposta = await _clienteRepository.CadastrarCliente(clienteRequest);
            return resposta;
        }

        public async Task<Cliente> CadastrarLogoutToken(Cliente request, string token)
        {
            return await _clienteRepository.CadastrarLogoutToken(request, token);
        }

        public async Task<Cliente> ConsultarCliente(Guid id)
        {
            return await _clienteRepository.ConsultarCliente(id);
        }

        public async Task<Cliente> ConsultarCredenciaisLogin(string email,string senha)
        {
            var cliente = await _clienteRepository.ConsultarCredenciaisLogin(email, senha);

            if (cliente != null)
                return cliente;
            else
                throw new LoginException("Cliente não encontrado!");
        }
    }
}
