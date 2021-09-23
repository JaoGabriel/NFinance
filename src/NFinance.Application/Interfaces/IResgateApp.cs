using System;
using System.Threading.Tasks;
using NFinance.Application.ViewModel.ResgatesViewModel;

namespace NFinance.Application.Interfaces
{
    public interface IResgateApp
    {
        public Task<ConsultarResgateViewModel.Response> ConsultarResgate(Guid idResgate);
        public Task<ConsultarResgatesViewModel.Response> ConsultarResgates(Guid idCliente);
        public Task<RealizarResgateViewModel.Response> RealizarResgate(RealizarResgateViewModel.Request request);
    }
}
