using Microsoft.EntityFrameworkCore;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Repository;
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

        public async Task<Gastos> AtualizarGasto(int id, Gastos gastos)
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

        public async Task<Gastos> ConsultarGasto(int id)
        {
            var gasto = await _context.Gastos.FirstOrDefaultAsync(i => i.Id == id);
            return gasto;
        }

        public async Task<int> ExcluirGasto(int id)
        {
            var gasto = await _context.Gastos.FirstOrDefaultAsync(i => i.Id == id);
            _context.Gastos.Remove(gasto);
            await UnitOfWork.Commit();
            return id;
        }

        public async Task<List<Gastos>> ListarGastos()
        {
            var gastosList = await _context.Gastos.ToListAsync();
            List<Gastos> listaGastos = new List<Gastos>();
            foreach (var gastos in gastosList)
                listaGastos.Add(gastos);

            return listaGastos;
        }
    }
}
