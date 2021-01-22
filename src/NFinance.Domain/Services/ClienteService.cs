using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Interfaces.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace NFinance.Domain.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }
        public async Task<Cliente> AtualizarCliente(Guid id, Cliente cliente)
        {
            return await _clienteRepository.AtualizarCliente(id,cliente);
        }

        public async Task<Cliente> CadastrarCliente(Cliente cliente)
        {
            return await _clienteRepository.CadastrarCliente(cliente);
        }

        public async Task<Cliente> ConsultarCliente(Guid id)
        {
            return await _clienteRepository.ConsultarCliente(id);
        }

        public async Task<List<Cliente>> ListarClientes()
        {
            return await _clienteRepository.ListarClientes();
        }
    }
}
