using System;
using NFinance.Domain;
using System.Threading.Tasks;
using NFinance.Application.Interfaces;
using NFinance.Domain.Interfaces.Services;
using NFinance.Application.ViewModel.ClientesViewModel;

namespace NFinance.Application
{
    public class ClienteApp : IClienteApp
    {
        private readonly IClienteService _clienteService;

        public ClienteApp(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        public async Task<AtualizarClienteViewModel.Response> AtualizarCliente(Guid id,AtualizarClienteViewModel.Request request)
        {
            var clienteDadosAtualizados = new Cliente(request.Nome,request.Cpf,request.Email,request.Senha);
            var clienteAtualizado = await _clienteService.AtualizarCliente(id,clienteDadosAtualizados);
            var resposta = new AtualizarClienteViewModel.Response(clienteAtualizado);
            return resposta;
        }

        public async Task<CadastrarClienteViewModel.Response> CadastrarCliente(CadastrarClienteViewModel.Request request)
        {
            var clienteNovo = new Cliente(request.Nome, request.Cpf, request.Email, request.Senha);
            var clienteCadastrado = await _clienteService.CadastrarCliente(clienteNovo);
            var resposta = new CadastrarClienteViewModel.Response(clienteCadastrado);
            return resposta;
        }

        public async Task<ConsultarClienteViewModel.Response> ConsultaCliente(Guid id)
        {
            var clienteAtualizado = await _clienteService.ConsultarCliente(id);
            var resposta = new ConsultarClienteViewModel.Response(clienteAtualizado);
            return resposta;
        }
    }
}
