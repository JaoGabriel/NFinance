using System;
using System.Text;
using System.Linq;
using NFinance.Domain;
using System.Globalization;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using NFinance.Domain.Interfaces.Repository;

namespace NFinance.Infra.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly BaseDadosContext _context;
        public IUnitOfWork UnitOfWork => _context;
        public ClienteRepository(BaseDadosContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public async Task<Cliente> AtualizarCliente(Guid id, Cliente cliente)
        {
            var clienteAtualizar = await _context.Cliente.FirstOrDefaultAsync(i => i.Id == id);
            var senhaCriptografada = HashValue(cliente.Senha);
            cliente.Senha = senhaCriptografada;
            _context.Entry(clienteAtualizar).CurrentValues.SetValues(cliente);
            await UnitOfWork.Commit();
            return cliente;
        }

        public async Task<Cliente> CadastrarCliente(Cliente cliente)
        {
            var senhaCriptografada = HashValue(cliente.Senha);
            cliente.Senha = senhaCriptografada;
            await _context.Cliente.AddAsync(cliente);
            await UnitOfWork.Commit();
            return cliente;
        }

        public async Task<Cliente> ConsultarCliente(Guid id)
        {
            var cliente = await _context.Cliente.FirstOrDefaultAsync(i => i.Id == id);
            return cliente;
        }

        public async Task<Cliente> ConsultarCredenciaisLogin(string email, string senha)
        {
            var users = await _context.Cliente.ToListAsync();
            var senhaCriptografada = HashValue(senha);
            var response = users.FirstOrDefault(x => x.Email.ToLower() == email.ToLower() && x.Senha == senhaCriptografada);

            return response;
        }

        public async Task<Cliente> CadastrarLogoutToken(Cliente clienteRequest,string token)
        {
            var cliente = await _context.Cliente.FirstOrDefaultAsync(i => i.Id == clienteRequest.Id);
            clienteRequest.Senha = cliente.Senha;
            clienteRequest.LogoutToken = token;
            _context.Entry(cliente).CurrentValues.SetValues(clienteRequest);
            await UnitOfWork.Commit();
            
            return clienteRequest;
        }

        static string HashValue(string value)
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
