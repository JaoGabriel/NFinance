using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces
{
    public interface IResgateService
    {
        Task<Resgate> RealizarResgate(int id, decimal valor, string motivo);
        Task<List<Resgate>> ListarResgates();
        Task<Resgate> ConsultarResgate(int id);
    }
}
