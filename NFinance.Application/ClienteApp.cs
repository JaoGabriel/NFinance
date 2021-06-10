using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
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
            var dadosClienteAtualizados = new Cliente(id,request.Nome,request.Cpf,request.Email,HashValue(request.Senha),null);
            var clienteAtualizado = await _clienteService.AtualizarCliente(dadosClienteAtualizados);
            var resposta = new AtualizarClienteViewModel.Response(clienteAtualizado);
            return resposta;
        }

        public async Task<CadastrarClienteViewModel.Response> CadastrarCliente(CadastrarClienteViewModel.Request request)
        {
            var clienteNovo = new Cliente(request.Nome, request.Cpf, request.Email, HashValue(request.Senha));
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
        
        private static string HashValue(string value)
        {
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
