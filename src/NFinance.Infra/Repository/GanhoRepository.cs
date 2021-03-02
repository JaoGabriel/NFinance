using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Repository;

namespace NFinance.Infra.Repository
{
    public class GanhoRepository : IGanhoRepository
    {
        private readonly BaseDadosContext _context;

        public GanhoRepository(BaseDadosContext baseDadosContext)
        {
            _context = baseDadosContext;
        }
        
        public IUnitOfWork UnitOfWork => _context;

        public void Dispose()
        {
            _context?.Dispose();
        }

        public async Task<Ganho> CadastrarGanho(Ganho ganho)
        {
            await _context.Ganhos.AddAsync(ganho);
            await UnitOfWork.Commit();
            return ganho;
        }

        public async Task<Ganho> AtualizarGanho(Guid id, Ganho ganho)
        {
            var ganhoAtualizar = await _context.Ganhos.FirstOrDefaultAsync(g => g.Id.Equals(id));
            _context.Entry(ganhoAtualizar).CurrentValues.SetValues(ganho);
            await UnitOfWork.Commit();
            return ganho;
        }

        public async Task<Ganho> ConsultarGanho(Guid id)
        {
            var ganho = await _context.Ganhos.FirstOrDefaultAsync(g => g.Id.Equals(id));
            return ganho;
        }

        public async Task<List<Ganho>> ConsultarGanhos(Guid idCliente)
        {
            var ganhos = await _context.Ganhos.ToListAsync();
            var listResponse = new List<Ganho>();

            foreach (var ganho in ganhos)
                if (ganho.IdCliente.Equals(idCliente))
                    listResponse.Add(ganho);

            return listResponse;
        }

        public async Task<bool> ExcluirGanho(Guid id)
        {
            var ganho = await _context.Ganhos.FirstOrDefaultAsync(i => i.Id == id);
            _context.Ganhos.Remove(ganho);
            await UnitOfWork.Commit();
            return true;
        }
    }
}