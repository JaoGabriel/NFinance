using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Repository
{
    public interface ITelaInicialRepository
    {
        public Task<List<Ganho>> GanhoMensal();

        public Task<List<Gasto>> GastoMensal();

        public Task<List<Investimento>> InvestimentoMensal();

        public Task<List<Resgate>> ResgateMensal();
    }
}
