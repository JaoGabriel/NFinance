using Microsoft.EntityFrameworkCore;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Repository;
using System.Collections.Generic;
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

        public async Task<Cliente> AtualiazarCliente(int id, Cliente cliente)
        {
            var clienteAtualizar = await _context.Cliente.FirstOrDefaultAsync(i => i.Id == id);
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

        public async Task<Cliente> ConsultarCliente(int id)
        {
            var cliente = await _context.Cliente.FirstOrDefaultAsync(i => i.Id == id);
            return cliente;
        }

        public async Task<List<Cliente>> ListarClientes()
        {
            var clienteList = await _context.Cliente.ToListAsync();
            List<Cliente> listaCliente = new List<Cliente>();
            foreach (var cliente in clienteList)
                listaCliente.Add(cliente);
            
            return listaCliente;
        }
    }
}
