using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NFinance.Domain.Interfaces.Repository
{
    public interface IGanhoRepository
    {
        Task<Ganho> CadastrarGanho(Ganho ganho);
        Task<Ganho> AtualizarGanho(Ganho ganho);
        Task<Ganho> ConsultarGanho(Guid id);
        Task<List<Ganho>> ConsultarGanhos(Guid idCliente);
        Task<bool> ExcluirGanho(Guid id);
    }
}