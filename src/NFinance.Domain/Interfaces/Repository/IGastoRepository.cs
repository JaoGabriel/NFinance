using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NFinance.Domain.Interfaces.Repository
{
    public interface IGastoRepository
    {
        Task<Gasto> CadastrarGasto(Gasto gastos);
        Task<Gasto> AtualizarGasto(Gasto gastos);
        Task<bool> ExcluirGasto(Guid id);
        Task<Gasto> ConsultarGasto(Guid id);
        Task<List<Gasto>> ConsultarGastos(Guid idCliente);
    }
}
