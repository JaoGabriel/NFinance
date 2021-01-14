using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces
{
    public interface IGastosService
    {
        Task<Gastos> CadastrarGasto(Gastos gastos);
        Task<Gastos> AtualizarGasto(int id,Gastos gastos);
        Task<Gastos> ExcluirGasto(int id);
        Task<Gastos> ConsultarGasto(int id);
        Task<List<Gastos>> ListarGastos();
    }
}
