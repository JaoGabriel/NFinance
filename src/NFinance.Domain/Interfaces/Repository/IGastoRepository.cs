using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Repository
{
    public interface IGastoRepository : IDisposable
    {
        Task<Gasto> CadastrarGasto(Gasto gastos);
        Task<Gasto> AtualizarGasto(Guid id, Gasto gastos);
        Task<bool> ExcluirGasto(Guid id);
        Task<Gasto> ConsultarGasto(Guid id);
        Task<List<Gasto>> ConsultarGastos(Guid idCliente);
    }
}
