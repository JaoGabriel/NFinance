using Microsoft.EntityFrameworkCore;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Cliente> CredenciaisLogin(string usuario, string senha)
        {
            var users = await _context.Cliente.ToListAsync();
            var senhaCriptografada = HashValue(senha);
            var response = users.Where(x => x.Email.ToLower() == usuario.ToLower() && x.Senha == senhaCriptografada).FirstOrDefault();

            return response;
        }

        static string HashValue(string value)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] hashBytes;
            using (HashAlgorithm hash = SHA1.Create())
                hashBytes = hash.ComputeHash(encoding.GetBytes(value));

            StringBuilder hashValue = new StringBuilder(hashBytes.Length * 2);
            foreach (byte b in hashBytes)
            {
                hashValue.AppendFormat(CultureInfo.InvariantCulture, "{0:X2}", b);
            }

            return hashValue.ToString();
        }
    }
}
