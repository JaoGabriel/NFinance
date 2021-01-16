using NFinance.Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Services
{
    public class ResgateService : IResgateService
    {
        public Task<Resgate> ConsultarResgate(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Resgate>> ListarResgates()
        {
            throw new System.NotImplementedException();
        }

        public Task<Resgate> RealizarResgate(int id, decimal valor, string motivo)
        {
            throw new System.NotImplementedException();
        }
    }
}
