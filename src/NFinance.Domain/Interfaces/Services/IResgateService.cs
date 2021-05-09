using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IResgateService
    {
        Task<Resgate> ConsultarResgate(Guid id);
        Task<List<Resgate>> ConsultarResgates(Guid idCliente);
        Task<Resgate> RealizarResgate(Resgate request);
    }
}
