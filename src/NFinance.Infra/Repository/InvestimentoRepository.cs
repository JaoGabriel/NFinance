using System;
using NFinance.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NFinance.Domain.Interfaces.Repository;

namespace NFinance.Infra.Repository
{
    public class InvestimentoRepository : IInvestimentoRepository
    {
        private readonly BaseDadosContext _context;
        
        public InvestimentoRepository(BaseDadosContext context)
        {
            _context = context;
        }

        public async Task<Investimento> AtualizarInvestimento(Investimento investimento)
        {
            var investimentoAtualizar = await _context.Investimento.FirstOrDefaultAsync(i => i.Id == investimento.Id);
            _context.Entry(investimentoAtualizar).CurrentValues.SetValues(investimento);
            await _context.SaveChangesAsync();
            return investimento;
        }

        public async Task<Investimento> ConsultarInvestimento(Guid id)
        {
            var investimento = await _context.Investimento.FirstOrDefaultAsync(i => i.Id == id);
            return investimento;
        }

        public async Task<Investimento> RealizarInvestimento(Investimento investimentos)
        {
            await _context.Investimento.AddAsync(investimentos);
            await _context.SaveChangesAsync();
            return investimentos;
        }

        public async Task<List<Investimento>> ConsultarInvestimentos(Guid idCliente)
        {
            var investimentos = await _context.Investimento.ToListAsync();
            var listResponse = new List<Investimento>();

            foreach (var investimento in investimentos)
                if (investimento.IdCliente.Equals(idCliente))
                    listResponse.Add(investimento);

            return listResponse;
        }
    }
}
