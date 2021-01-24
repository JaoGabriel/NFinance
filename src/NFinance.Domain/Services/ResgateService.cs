using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Interfaces.Services;
using NFinance.Model.ResgatesViewModel;
using System;
using System.Threading.Tasks;

namespace NFinance.Domain.Services
{
    public class ResgateService : IResgateService
    {
        private readonly IResgateRepository _resgateRepository;
        private readonly IInvestimentosService _investimentosService;

        public ResgateService(IResgateRepository resgateRepository, IInvestimentosService investimentosService)
        {
            _resgateRepository = resgateRepository;
            _investimentosService = investimentosService;
        }

        public async Task<ConsultarResgateViewModel.Response> ConsultarResgate(Guid id)
        {
            var resgate = await _resgateRepository.ConsultarResgate(id);
            var investimento = await _investimentosService.ConsultarInvestimento(resgate.IdInvestimento);
            var response = new ConsultarResgateViewModel.Response() { Id = resgate.Id, Valor = resgate.Valor, DataResgate = resgate.DataResgate, MotivoResgate = resgate.MotivoResgate, Investimento = investimento };
            return response;
        }

        public async Task<ListarResgatesViewModel.Response> ListarResgates()
        {
            var listaResgate = await _resgateRepository.ListarResgates();
            var response = new ListarResgatesViewModel.Response(listaResgate);
            return response;
        }

        public async Task<RealizarResgateViewModel.Response> RealizarResgate(RealizarResgateViewModel.Request request)
        {
            var resgateRequest = new Resgate(request);
            var investimento = await _investimentosService.ConsultarInvestimento(resgateRequest.IdInvestimento);
            var resgate = await _resgateRepository.RealizarResgate(resgateRequest);
            var response = new RealizarResgateViewModel.Response() { Id = resgate.Id, Valor = resgate.Valor, DataResgate = resgate.DataResgate, MotivoResgate = resgate.MotivoResgate, Investimento = investimento };
            return response;
        }
    }
}
