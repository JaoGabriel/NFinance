using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Repository
{
    public interface IGastosRepository : IDisposable
    {
        Task<Gastos> CadastrarGasto(Gastos gastos);
        Task<Gastos> AtualizarGasto(Guid id, Gastos gastos);
        Task<bool> ExcluirGasto(Guid id);
        Task<Gastos> ConsultarGasto(Guid id);
        Task<List<Gastos>> ConsultarGastos(Guid idCliente);
    }
}
