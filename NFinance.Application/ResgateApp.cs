using System;
using NFinance.Domain;
using System.Threading.Tasks;
using NFinance.Application.Interfaces;
using NFinance.Domain.Interfaces.Services;
using NFinance.Application.ViewModel.ResgatesViewModel;

namespace NFinance.Application
{
    public class ResgateApp : IResgateApp
    {
        private readonly IResgateService _resgateService;

        public ResgateApp(IResgateService resgateService)
        {
            _resgateService = resgateService;
        }

        public async Task<ConsultarResgateViewModel.Response> ConsultarResgate(Guid idResgate)
        {
            var resgate = await _resgateService.ConsultarResgate(idResgate);
            var resposta = new ConsultarResgateViewModel.Response(resgate);
            return resposta;
        }

        public async Task<ConsultarResgatesViewModel.Response> ConsultarResgates(Guid idCliente)
        {
            var resgates = await _resgateService.ConsultarResgates(idCliente);
            var resposta = new ConsultarResgatesViewModel.Response(resgates);
            return resposta;
        }

        public async Task<RealizarResgateViewModel.Response> RealizarResgate(RealizarResgateViewModel.Request request)
        {
            var realizarResgate = new Resgate(request.IdInvestimento,request.IdCliente,request.Valor,request.MotivoResgate,request.DataResgate);
            var resgateRealizado = await _resgateService.RealizarResgate(realizarResgate);
            var resposta = new RealizarResgateViewModel.Response(resgateRealizado);
            return resposta;
        }
    }
}
