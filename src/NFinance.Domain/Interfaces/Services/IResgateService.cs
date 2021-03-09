using NFinance.ViewModel.ResgatesViewModel;
using System;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IResgateService
    {
        Task<RealizarResgateViewModel.Response> RealizarResgate(RealizarResgateViewModel.Request request);
        Task<ConsultarResgateViewModel.Response> ConsultarResgate(Guid id);
        Task<ConsultarResgatesViewModel.Response> ConsultarResgates(Guid idCliente);
    }
}
