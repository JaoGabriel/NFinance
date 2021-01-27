using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Resgate;
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
            if (Guid.Empty.Equals(id)) throw new IdException();

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
            if (Guid.Empty.Equals(request.IdInvestimento)) throw new IdException();
            if (string.IsNullOrWhiteSpace(request.MotivoResgate) == true) throw new MotivoResgateException("Motivo nao deve ser branco,vazio ou nulo");
            if (request.Valor <= 0) throw new ValorException("Valor deve ser maior que zero");
            if (request.DataResgate > DateTime.MaxValue.AddYears(-7899) || request.DataResgate < DateTime.MinValue.AddYears(1949)) throw new DataResgateException();

            var resgateRequest = new Resgate(request);
            var investimento = await _investimentosService.ConsultarInvestimento(resgateRequest.IdInvestimento);
            var resgate = await _resgateRepository.RealizarResgate(resgateRequest);
            var response = new RealizarResgateViewModel.Response() { Id = resgate.Id, Valor = resgate.Valor, DataResgate = resgate.DataResgate, MotivoResgate = resgate.MotivoResgate, Investimento = investimento };
            return response;
        }
    }
}
