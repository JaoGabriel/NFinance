using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Repository
{
    public interface IGastosRepository : IDisposable
    {
        Task<Gastos> CadastrarGasto(Gastos gastos);
        Task<Gastos> AtualizarGasto(int id, Gastos gastos);
        Task<int> ExcluirGasto(int id);
        Task<Gastos> ConsultarGasto(int id);
        Task<List<Gastos>> ListarGastos();
    }
}
