using NFinance.Model.ResgatesViewModel;
using System;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IResgateService
    {
        Task<RealizarResgateViewModel.Response> RealizarResgate(RealizarResgateViewModel.Request request);
        Task<ListarResgatesViewModel.Response> ListarResgates();
        Task<ConsultarResgateViewModel.Response> ConsultarResgate(Guid id);
    }
}
