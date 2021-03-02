using Microsoft.EntityFrameworkCore;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Infra.Repository
{
    public class InvestimentosRepository : IInvestimentosRepository
    {
        private readonly BaseDadosContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public InvestimentosRepository(BaseDadosContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
        public async Task<Investimentos> AtualizarInvestimento(Guid id, Investimentos investimento)
        {
            var investimentoAtualizar = await _context.Investimentos.FirstOrDefaultAsync(i => i.Id == id);
            _context.Entry(investimentoAtualizar).CurrentValues.SetValues(investimento);
            await UnitOfWork.Commit();
            return investimento;
        }

        public async Task<Investimentos> ConsultarInvestimento(Guid id)
        {
            var investimento = await _context.Investimentos.FirstOrDefaultAsync(i => i.Id == id);
            return investimento;
        }

        public async Task<Investimentos> RealizarInvestimento(Investimentos investimentos)
        {
            await _context.Investimentos.AddAsync(investimentos);
            await UnitOfWork.Commit();
            return investimentos;
        }

        public async Task<List<Investimentos>> ConsultarInvestimentos(Guid idCliente)
        {
            var investimentos = await _context.Investimentos.ToListAsync();
            var listResponse = new List<Investimentos>();

            foreach (var investimento in investimentos)
                if (investimento.IdCliente.Equals(idCliente))
                    listResponse.Add(investimento);

            return listResponse;
        }
    }
}
