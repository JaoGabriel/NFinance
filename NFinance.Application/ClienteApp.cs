using System;
using System.Text;
using NFinance.Domain;
using System.Globalization;
using System.Threading.Tasks;
using NFinance.Domain.Exceptions;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using NFinance.Application.Interfaces;
using NFinance.Domain.Exceptions.Cliente;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Application.ViewModel.ClientesViewModel;
using NFinance.Application.ViewModel.AutenticacaoViewModel;
using NFinance.Infra.Identidade;

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

        public async Task CadastraLogoutToken(LogoutViewModel logout)
        {
            await _clienteRepository.CadastrarLogoutToken(logout.IdCliente,logout.Token);
        }

        public async Task<CadastrarClienteViewModel.Response> CadastrarCliente(CadastrarClienteViewModel.Request request)
        {
            //var user = new Usuario {Email = request.Email, UserName = request.Email};
            //var userResult = await _userManager.CreateAsync(user, request.Senha);
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
