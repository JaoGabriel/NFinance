using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IGastosService
    {
        Task<Gastos> CadastrarGasto(Gastos gastos);
        Task<Gastos> AtualizarGasto(Guid id,Gastos gastos);
        Task<Guid> ExcluirGasto(Guid id);
        Task<Gastos> ConsultarGasto(Guid id);
        Task<List<Gastos>> ListarGastos();
    }
}
