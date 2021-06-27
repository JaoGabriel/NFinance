using System;
using System.Linq;
using NFinance.Domain;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NFinance.Domain.Interfaces.Repository;

namespace NFinance.Infra.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly BaseDadosContext _context;
        public ClienteRepository(BaseDadosContext context)
        {
            _context = context;
        }

        public async Task<Cliente> AtualizarCliente(Cliente cliente)
        {
            var clienteAtualizar = await _context.Cliente.FirstOrDefaultAsync(i => i.Id == cliente.Id);
            _context.Entry(clienteAtualizar).CurrentValues.SetValues(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente> CadastrarCliente(Cliente cliente)
        {
            await _context.Cliente.AddAsync(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente> ConsultarCliente(Guid id)
        {
            var cliente = await _context.Cliente.FirstOrDefaultAsync(i => i.Id == id);
            return cliente;
        }

        public async Task CadastrarLogoutToken(Guid idCliente,string token)
        {
            var clienteToken = await _context.Cliente.FirstOrDefaultAsync(i => i.Id == idCliente);
            var cliente = new Cliente(clienteToken.Id, clienteToken.Nome, clienteToken.Email, clienteToken.Senha, token);
            _context.Entry(clienteToken).CurrentValues.SetValues(cliente);
            await _context.SaveChangesAsync(); 
        }
    }
}
