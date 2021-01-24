using Microsoft.EntityFrameworkCore;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Infra.Repository
{
    public class ResgateRepository : IResgateRepository
    {
        private readonly BaseDadosContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public ResgateRepository(BaseDadosContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public async Task<Resgate> ConsultarResgate(Guid id)
        {
            return await _context.Resgate.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<Resgate>> ListarResgates()
        {
            return await _context.Resgate.ToListAsync();
        }

        public async Task<Resgate> RealizarResgate(Resgate resgate)
        {
            var investimento = await _context.Investimentos.FindAsync(resgate.IdInvestimento);
            var resgateResponse = new Resgate() {Valor = resgate.Valor, MotivoResgate = resgate.MotivoResgate, DataResgate = DateTime.UtcNow};
            await _context.Resgate.AddAsync(resgateResponse);
            _context.Investimentos.Remove(investimento);
            await UnitOfWork.Commit();
            return resgateResponse;
        }
    }
}
