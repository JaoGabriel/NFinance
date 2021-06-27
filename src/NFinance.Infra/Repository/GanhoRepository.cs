using System;
using NFinance.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Ganho> CadastrarGanho(Ganho ganho)
        {
            await _context.Ganho.AddAsync(ganho);
            await _context.SaveChangesAsync();
            return ganho;
        }

        public async Task<Ganho> AtualizarGanho(Ganho ganho)
        {
            var ganhoAtualizar = await _context.Ganho.FirstOrDefaultAsync(g => g.Id.Equals(ganho.Id));
            _context.Entry(ganhoAtualizar).CurrentValues.SetValues(ganho);
            await _context.SaveChangesAsync();
            return ganho;
        }

        public async Task<Ganho> ConsultarGanho(Guid id)
        {
            var ganho = await _context.Ganho.FirstOrDefaultAsync(g => g.Id.Equals(id));
            return ganho;
        }

        public async Task<List<Ganho>> ConsultarGanhos(Guid idCliente)
        {
            var ganhos = await _context.Ganho.ToListAsync();
            var listResponse = new List<Ganho>();

            foreach (var ganho in ganhos)
                if (ganho.IdCliente.Equals(idCliente))
                    listResponse.Add(ganho);

            return listResponse;
        }

        public async Task<bool> ExcluirGanho(Guid id)
        {
            var ganho = await _context.Ganho.FirstOrDefaultAsync(i => i.Id == id);
            _context.Ganho.Remove(ganho);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}