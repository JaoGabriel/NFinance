using System;
using NFinance.Domain;
using System.Threading.Tasks;
using NFinance.Domain.Exceptions;
using NFinance.Application.Interfaces;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Application.ViewModel.ClientesViewModel;
using NFinance.Domain.Repository;

namespace NFinance.Application
{
    public class ClienteApp : IClienteApp
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ClienteApp(IClienteRepository clienteRepository, IUsuarioRepository usuarioRepository)
        {
            _clienteRepository = clienteRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<AtualizarClienteViewModel.Response> AtualizarDadosCadastrais(Guid id, AtualizarClienteViewModel.Request request)
        {
            var dadosClienteAtualizados = new Cliente(id, request.Nome, request.Cpf, request.Email);
            var clienteAtualizado = await _clienteRepository.AtualizarCliente(dadosClienteAtualizados);
            var resposta = new AtualizarClienteViewModel.Response(clienteAtualizado);
            return resposta;
        }

        public async Task<CadastrarClienteViewModel.Response> CadastrarCliente(CadastrarClienteViewModel.Request request)
        {
            var user = await _usuarioRepository.CadastrarUsuario(request.Email,request.Senha);
            var clienteNovo = new Cliente(request.Nome, request.Cpf, request.Email,user);
            var clienteCadastrado = await _clienteRepository.CadastrarCliente(clienteNovo);
            var resposta = new CadastrarClienteViewModel.Response(clienteCadastrado);
            return resposta;
        }

        public async Task<ConsultarClienteViewModel.Response> ConsultaCliente(Guid id)
        {
            if (Guid.Empty.Equals(id)) throw new IdException("Id invalido!");
            
            var clienteAtualizado = await _clienteRepository.ConsultarCliente(id);
            var resposta = new ConsultarClienteViewModel.Response(clienteAtualizado);
            return resposta;
        }
    }
}
