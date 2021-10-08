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
        
        public ResgateRepository(BaseDadosContext context)
        {
            _context = context;
        }

        public async Task<Resgate> ConsultarResgate(Guid id)
        {
            return await _context.Resgate.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Resgate> RealizarResgate(Resgate resgate)
        {
            var investimento = await _context.Investimento.FindAsync(resgate.IdInvestimento);
            var resgateResponse = new Resgate(resgate.IdInvestimento,resgate.IdCliente,resgate.Valor, resgate.MotivoResgate,DateTime.UtcNow);
            await _context.Resgate.AddAsync(resgateResponse);
            _context.Investimento.Remove(investimento);
            await _context.SaveChangesAsync();
            return resgateResponse;
        }

        public async Task<List<Resgate>> ConsultarResgates(Guid idCliente)
        {
            var resgates = await _context.Resgate.ToListAsync();
            var listResponse = new List<Resgate>();

            foreach (var resgate in resgates)
                if (resgate.IdCliente.Equals(idCliente))
                    listResponse.Add(resgate);

            return listResponse;
        }
    }
}
