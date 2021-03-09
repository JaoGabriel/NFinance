using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Repository
{
    public interface IGanhoRepository : IDisposable
    {
        Task<Ganho> CadastrarGanho(Ganho ganho);
        Task<Ganho> AtualizarGanho(Guid id, Ganho ganho);
        Task<Ganho> ConsultarGanho(Guid id);
        Task<List<Ganho>> ConsultarGanhos(Guid idCliente);
        Task<bool> ExcluirGanho(Guid id);
    }
}