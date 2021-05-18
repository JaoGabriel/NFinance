using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IGastoService
    {
        Task<Gasto> AtualizarGasto(Gasto request);
        Task<Gasto> CadastrarGasto(Gasto request);
        Task<Gasto> ConsultarGasto(Guid id);
        Task<List<Gasto>> ConsultarGastos(Guid idCliente);
        Task<bool> ExcluirGasto(Gasto request);
    }
}
