using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Interfaces.Repository;
using System.Threading.Tasks;
using System;
using NFinance.Model.ClientesViewModel;

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
            var cliente = new Cliente(id, clienteRequest);
            var atualizado = await _clienteRepository.AtualizarCliente(id, cliente);
            var response = new AtualizarClienteViewModel.Response() {Id = atualizado.Id,Nome = atualizado.Nome, RendaMensal = atualizado.RendaMensal };
            return response;
        }

        public async Task<CadastrarClienteViewModel.Response> CadastrarCliente(CadastrarClienteViewModel.Request clienteRequest)
        {
            var cliente = new Cliente(clienteRequest);
            var novoCliente = await _clienteRepository.CadastrarCliente(cliente);
            var response = new CadastrarClienteViewModel.Response() {Id = novoCliente.Id,Nome = novoCliente.Nome,RendaMensal = novoCliente.RendaMensal };
            return response;
        }

        public async Task<ConsultarClienteViewModel.Response> ConsultarCliente(Guid id)
        {
            var clienteConsulta = await _clienteRepository.ConsultarCliente(id);
            var response = new ConsultarClienteViewModel.Response() { Id = clienteConsulta.Id, Nome = clienteConsulta.Nome, RendaMensal = clienteConsulta.RendaMensal };
            return response;
        }

        public async Task<ListarClientesViewModel.Response> ListarClientes()
        {
            var listaClientes = await _clienteRepository.ListarClientes();
            var response = new ListarClientesViewModel.Response(listaClientes);
            return response;
        }
    }
}
