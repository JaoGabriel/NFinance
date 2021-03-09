using Microsoft.EntityFrameworkCore;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Infra.Repository
{
    public class GastoRepository : IGastoRepository
    {
        private readonly BaseDadosContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public GastoRepository(BaseDadosContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }


        public async Task<Gasto> AtualizarGasto(Guid id, Gasto gastos)
        {
            var gastoAtualizar = await _context.Gasto.FirstOrDefaultAsync(i => i.Id == id);
            _context.Entry(gastoAtualizar).CurrentValues.SetValues(gastos);
            await UnitOfWork.Commit();
            return gastos;
        }

        public async Task<Gasto> CadastrarGasto(Gasto gastos)
        {
            await _context.Gasto.AddAsync(gastos);
            await UnitOfWork.Commit();
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
            await UnitOfWork.Commit();
            return true;
        }
    }
}
