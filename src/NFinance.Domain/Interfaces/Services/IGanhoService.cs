using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IGanhoService
    {
        Task<Ganho> CadastrarGanho(Ganho request);
        Task<Ganho> AtualizarGanho(Ganho request);
        Task<Ganho> ConsultarGanho(Guid id);
        Task<List<Ganho>> ConsultarGanhos(Guid idCliente);
        Task<bool> ExcluirGanho(Ganho request);
    }
}