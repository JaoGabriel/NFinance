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
            _context.Entry(clienteAtualizar).CurrentValues.SetValues(cliente);
            await UnitOfWork.Commit();
            return cliente;
        }

        public async Task<Cliente> CadastrarCliente(Cliente cliente)
        {
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
            var response = usuarios.FirstOrDefault(x => x.Email.ToLower() == usuario.ToLower() && x.Senha == senha);

            return response;
        }

        public async Task CadastrarLogoutToken(Cliente cliente)
        {
            var clienteToken = await _context.Cliente.FirstOrDefaultAsync(i => i.Id == cliente.Id);
            _context.Entry(clienteToken).CurrentValues.SetValues(cliente);
            await UnitOfWork.Commit(); 
        }
    }
}
