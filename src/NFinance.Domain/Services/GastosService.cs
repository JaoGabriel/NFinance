using NFinance.Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Services
{
    public class GastosService : IGastosService
    {
        public Task<Gastos> AtualizarGasto(int id, Gastos gastos)
        {
            throw new System.NotImplementedException();
        }

        public Task<Gastos> CadastrarGasto(Gastos gastos)
        {
            throw new System.NotImplementedException();
        }

        public Task<Gastos> ConsultarGasto(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Gastos> ExcluirGasto(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Gastos>> ListarGastos()
        {
            throw new System.NotImplementedException();
        }
    }
}
