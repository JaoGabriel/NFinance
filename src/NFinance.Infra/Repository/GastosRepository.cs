using Microsoft.EntityFrameworkCore;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Infra.Repository
{
    public class GastosRepository : IGastosRepository
    {
        private readonly BaseDadosContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public GastosRepository(BaseDadosContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }


        public async Task<Gastos> AtualizarGasto(Guid id, Gastos gastos)
        {
            var gastoAtualizar = await _context.Gastos.FirstOrDefaultAsync(i => i.Id == id);
            _context.Entry(gastoAtualizar).CurrentValues.SetValues(gastos);
            await UnitOfWork.Commit();
            return gastos;
        }

        public async Task<Gastos> CadastrarGasto(Gastos gastos)
        {
            await _context.Gastos.AddAsync(gastos);
            await UnitOfWork.Commit();
            return gastos;
        }

        public async Task<Gastos> ConsultarGasto(Guid id)
        {
            var gasto = await _context.Gastos.FirstOrDefaultAsync(i => i.Id == id);
            return gasto;
        }

        public async Task<List<Gastos>> ConsultarGastos(Guid idCliente)
        {
            var gastos = await _context.Gastos.ToListAsync();
            var listResponse = new List<Gastos>();

            foreach (var gasto in gastos)
                if (gasto.IdCliente.Equals(idCliente))
                    listResponse.Add(gasto);

            return listResponse;
        }

        public async Task<bool> ExcluirGasto(Guid id)
        {
            var gasto = await _context.Gastos.FirstOrDefaultAsync(i => i.Id == id);
            _context.Gastos.Remove(gasto);
            await UnitOfWork.Commit();
            return true;
        }
    }
}
