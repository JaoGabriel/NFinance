using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Services
{
    public class GastosService : IGastosService
    {
        private readonly IGastosRepository _gastosRepository;
        public GastosService(IGastosRepository gastosRepository)
        {
            _gastosRepository = gastosRepository;
        }

        public Task<Gastos> AtualizarGasto(int id, Gastos gastos)
        {
            return _gastosRepository.AtualizarGasto(id,gastos);
        }

        public Task<Gastos> CadastrarGasto(Gastos gastos)
        {
            return _gastosRepository.CadastrarGasto(gastos);
        }

        public Task<Gastos> ConsultarGasto(int id)
        {
            return _gastosRepository.ConsultarGasto(id);
        }

        public Task<int> ExcluirGasto(int id)
        {
            return _gastosRepository.ExcluirGasto(id);
        }

        public Task<List<Gastos>> ListarGastos()
        {
            return _gastosRepository.ListarGastos();
        }
    }
}
