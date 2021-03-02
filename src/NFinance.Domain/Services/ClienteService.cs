using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Interfaces.Repository;
using System.Threading.Tasks;
using System;
using NFinance.Domain.Exceptions.Cliente;
using NFinance.Domain.Exceptions;
using NFinance.Domain.ViewModel.ClientesViewModel;

namespace NFinance.Domain.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }
        public async Task<AtualizarClienteViewModel.Response> AtualizarCliente(Guid id, AtualizarClienteViewModel.Request clienteRequest)
        {
            if (Guid.Empty.Equals(id)) throw new IdException("Id cliente invalido");
            if (string.IsNullOrWhiteSpace(clienteRequest.Nome)) throw new NomeClienteException();
            if (string.IsNullOrWhiteSpace(clienteRequest.Cpf)) throw new CpfClienteException();
            if (string.IsNullOrWhiteSpace(clienteRequest.Email)) throw new EmailClienteException();
            if (string.IsNullOrWhiteSpace(clienteRequest.Senha)) throw new SenhaClienteException();

            var cliente = new Cliente(id, clienteRequest);
            var atualizado = await _clienteRepository.AtualizarCliente(id, cliente);
            var response = new AtualizarClienteViewModel.Response() {Id = atualizado.Id,Nome = atualizado.Nome, Cpf = atualizado.CPF,Email = atualizado.Email};
            return response;
        }

        public async Task<CadastrarClienteViewModel.Response> CadastrarCliente(CadastrarClienteViewModel.Request clienteRequest)
        {
            if (string.IsNullOrWhiteSpace(clienteRequest.Nome)) throw new NomeClienteException();
            if (string.IsNullOrWhiteSpace(clienteRequest.Cpf)) throw new CpfClienteException();
            if (string.IsNullOrWhiteSpace(clienteRequest.Email)) throw new EmailClienteException();
            if (string.IsNullOrWhiteSpace(clienteRequest.Senha)) throw new SenhaClienteException();

            var cliente = new Cliente(clienteRequest);
            var novoCliente = await _clienteRepository.CadastrarCliente(cliente);
            var response = new CadastrarClienteViewModel.Response() {Id = novoCliente.Id,Nome = novoCliente.Nome,Cpf = novoCliente.CPF, Email = novoCliente.Email};
            return response;
        }

        public async Task<ConsultarClienteViewModel.Response> ConsultarCliente(Guid id)
        {
            if (Guid.Empty.Equals(id)) throw new IdException("Id cliente invalido");
            
            var clienteConsulta = await _clienteRepository.ConsultarCliente(id);
            var response = new ConsultarClienteViewModel.Response() { Id = clienteConsulta.Id, Nome = clienteConsulta.Nome, Cpf = clienteConsulta.CPF,Email = clienteConsulta.Email};
            return response;
        }
    }
}
