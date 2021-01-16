using Microsoft.EntityFrameworkCore;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Repository;
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
        public async Task<Investimentos> AtualizarInvestimento(int id, Investimentos investimento)
        {
            var investimentoAtualizar = await _context.Gastos.FirstOrDefaultAsync(i => i.Id == id);
            _context.Entry(investimentoAtualizar).CurrentValues.SetValues(investimento);
            await UnitOfWork.Commit();
            return investimento;
        }

        public async Task<Investimentos> ConsultarInvestimento(int id)
        {
            var cliente = await _context.Cliente.FirstOrDefaultAsync(i => i.Id == id);
            var investimento = await _context.Investimentos.FirstOrDefaultAsync(i => i.Id == cliente.Id);
            return investimento;
        }


        public async Task<List<Investimentos>> ListarInvestimentos()
        {
            var investList = await _context.Investimentos.ToListAsync();
            List<Investimentos> listaInvestimentos = new List<Investimentos>();
            foreach (var investimentos in investList)
                listaInvestimentos.Add(investimentos);

            return listaInvestimentos;
        }

        public async Task<Investimentos> RealizarInvestimento(Investimentos investimentos)
        {
            await _context.Investimentos.AddAsync(investimentos);
            ListaInvestimentosCliente(investimentos);
            await UnitOfWork.Commit();
            return investimentos;
        }

        public List<Investimentos> ListaInvestimentosCliente(Investimentos investimentos)
        {
            List<Investimentos> investCliente = new List<Investimentos>();
             investCliente.Add(investimentos);
            return investCliente;
        }
    }
}
