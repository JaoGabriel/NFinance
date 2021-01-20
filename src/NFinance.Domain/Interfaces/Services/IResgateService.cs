using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IResgateService
    {
        Task<Resgate> RealizarResgate(Guid idInvestimento,Resgate resgate);
        Task<List<Resgate>> ListarResgates();
        Task<Resgate> ConsultarResgate(Guid id);
    }
}
