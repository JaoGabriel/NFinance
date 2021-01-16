using NFinance.Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Services
{
    public class InvestimentosService : IInvestimentosService
    {
        public Task<Investimentos> AtualizarInvestimento(int id, Investimentos investimento)
        {
            throw new System.NotImplementedException();
        }

        public Task<Investimentos> ConsultarInvestimento(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Investimentos>> ListarInvestimentos()
        {
            throw new System.NotImplementedException();
        }

        public Task<Investimentos> RealizarInvestimento(Investimentos investimentos)
        {
            throw new System.NotImplementedException();
        }
    }
}
