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

        public async Task<Cliente> AtualizarCliente(Cliente cliente)
        {
            var clienteAtualizar = await _context.Cliente.FirstOrDefaultAsync(i => i.Id == cliente.Id);
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

        public async Task<Cliente> ConsultarCredenciaisLogin(string usuario, string senha)
        {
            var usuarios = await _context.Cliente.ToListAsync();
            var senhaCriptografada = HashValue(senha);
            var response = usuarios.FirstOrDefault(x => x.Email.ToLower() == usuario.ToLower() && x.Senha == senhaCriptografada);

            return response;
        }

        public async Task<Cliente> CadastrarLogoutToken(Cliente cliente,string token)
        {
            var clienteToken = await _context.Cliente.FirstOrDefaultAsync(i => i.Id == cliente.Id);
            cliente.Senha = clienteToken.Senha;
            cliente.LogoutToken = token;
            _context.Entry(clienteToken).CurrentValues.SetValues(cliente);
            await UnitOfWork.Commit();
            
            return cliente;
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
