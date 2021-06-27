using System;
using NFinance.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NFinance.Domain.Interfaces.Repository;

namespace NFinance.Infra.Repository
{
    public class GastoRepository : IGastoRepository
    {
        private readonly BaseDadosContext _context;

        public GastoRepository(BaseDadosContext context)
        {
            _context = context;
        }
        
        public async Task<Gasto> AtualizarGasto(Gasto gastos)
        {
            var gastoAtualizar = await _context.Gasto.FirstOrDefaultAsync(i => i.Id == gastos.Id);
            _context.Entry(gastoAtualizar).CurrentValues.SetValues(gastos);
            await _context.SaveChangesAsync();
            return gastos;
        }

        public async Task<Gasto> CadastrarGasto(Gasto gastos)
        {
            await _context.Gasto.AddAsync(gastos);
            await _context.SaveChangesAsync();
            return gastos;
        }

        public async Task<Gasto> ConsultarGasto(Guid id)
        {
            var gasto = await _context.Gasto.FirstOrDefaultAsync(i => i.Id == id);
            return gasto;
        }

        public async Task<List<Gasto>> ConsultarGastos(Guid idCliente)
        {
            var gastos = await _context.Gasto.ToListAsync();
            var listResponse = new List<Gasto>();

            foreach (var gasto in gastos)
                if (gasto.IdCliente.Equals(idCliente))
                    listResponse.Add(gasto);

            return listResponse;
        }

        public async Task<bool> ExcluirGasto(Guid id)
        {
            var gasto = await _context.Gasto.FirstOrDefaultAsync(i => i.Id == id);
            _context.Gasto.Remove(gasto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
