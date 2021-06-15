using System;
using System.Text;
using NFinance.Domain;
using System.Globalization;
using System.Threading.Tasks;
using System.Security.Cryptography;
using NFinance.Application.Interfaces;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Exceptions.Autenticacao;
using NFinance.Application.ViewModel.ClientesViewModel;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Cliente;

namespace NFinance.Application
{
    public class ClienteApp : IClienteApp
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteApp(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<AtualizarClienteViewModel.Response> AtualizarCliente(Guid id,AtualizarClienteViewModel.Request request)
        {
            var dadosClienteAtualizados = new Cliente(id,request.Nome,request.Cpf,request.Email,HashValue(request.Senha));
            var clienteAtualizado = await _clienteRepository.AtualizarCliente(dadosClienteAtualizados);
            var resposta = new AtualizarClienteViewModel.Response(clienteAtualizado);
            return resposta;
        }

        public async Task<CadastrarClienteViewModel.Response> CadastrarCliente(CadastrarClienteViewModel.Request request)
        {
            var clienteNovo = new Cliente(request.Nome, request.Cpf, request.Email, HashValue(request.Senha));
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

        public async Task<Cliente> ConsultarCredenciaisLogin(string email, string senha)
        {
            var cliente = await _clienteRepository.ConsultarCredenciaisLogin(email, senha);

            if (cliente != null)
                return cliente;
            else
                throw new LoginException("Cliente não encontrado!");
        }

        public async Task CadastrarLogoutToken(Cliente cliente)
        {
            await _clienteRepository.CadastrarLogoutToken(cliente);
        }

        private static string HashValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new SenhaClienteException("Senha nao pode ser nula, vazia ou em branco");
            
            var encoding = new UnicodeEncoding();
            byte[] hashBytes;
            using (HashAlgorithm hash = SHA256.Create())
                hashBytes = hash.ComputeHash(encoding.GetBytes(value));

            var hashValue = new StringBuilder(hashBytes.Length * 2);
            foreach (byte b in hashBytes)
            {
                hashValue.AppendFormat(CultureInfo.InvariantCulture, "{0:X2}", b);
            }

            return hashValue.ToString();
        }
    }
}
