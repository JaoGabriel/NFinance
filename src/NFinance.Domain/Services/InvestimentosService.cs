using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Services
{
    public class InvestimentosService : IInvestimentosService
    {
        private readonly IInvestimentosRepository _investimentosRepository;

        public InvestimentosService(IInvestimentosRepository investimentosRepository)
        {
            _investimentosRepository = investimentosRepository;
        }

        public Task<Investimentos> AtualizarInvestimento(int id, Investimentos investimento)
        {
            return _investimentosRepository.AtualizarInvestimento(id,investimento);
        }

        public Task<Investimentos> ConsultarInvestimento(int id)
        {
            return _investimentosRepository.ConsultarInvestimento(id);
        }

        public Task<List<Investimentos>> ListarInvestimentos()
        {
            return _investimentosRepository.ListarInvestimentos();
        }

        public Task<Investimentos> RealizarInvestimento(Investimentos investimentos)
        {
            return _investimentosRepository.RealizarInvestimento(investimentos);
        }
    }
}
