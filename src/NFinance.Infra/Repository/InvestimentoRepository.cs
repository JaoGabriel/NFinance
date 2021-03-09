using Microsoft.EntityFrameworkCore;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Infra.Repository
{
    public class InvestimentoRepository : IInvestimentoRepository
    {
        private readonly BaseDadosContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public InvestimentoRepository(BaseDadosContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
        public async Task<Investimento> AtualizarInvestimento(Guid id, Investimento investimento)
        {
            var investimentoAtualizar = await _context.Investimento.FirstOrDefaultAsync(i => i.Id == id);
            _context.Entry(investimentoAtualizar).CurrentValues.SetValues(investimento);
            await UnitOfWork.Commit();
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
            await UnitOfWork.Commit();
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
