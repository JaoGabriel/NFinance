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
            if (Guid.Empty.Equals(id)) throw new IdException("Id resgate invalido");

            var resgate = await _resgateRepository.ConsultarResgate(id);
            var investimento = await _investimentosService.ConsultarInvestimento(resgate.IdInvestimento);
            var response = new ConsultarResgateViewModel.Response { Id = resgate.Id, Valor = resgate.Valor, DataResgate = resgate.DataResgate, MotivoResgate = resgate.MotivoResgate, Investimento = investimento, IdCliente = resgate.IdCliente };
            return response;
        }

        public async Task<ConsultarResgatesViewModel.Response> ConsultarResgates(Guid idCliente)
        {
            if (Guid.Empty.Equals(idCliente)) throw new IdException("Id cliente invalido");

            var consultarResgates = await _resgateRepository.ConsultarResgates(idCliente);
            var response = new ConsultarResgatesViewModel.Response(consultarResgates);
            return response;
        }

        public async Task<RealizarResgateViewModel.Response> RealizarResgate(RealizarResgateViewModel.Request request)
        {
            if (Guid.Empty.Equals(request.IdCliente)) throw new IdException("Id cliente invalido");
            if (Guid.Empty.Equals(request.IdInvestimento)) throw new IdException("Id investimento invalido");
            if (string.IsNullOrWhiteSpace(request.MotivoResgate)) throw new MotivoResgateException("Motivo nao deve ser branco,vazio ou nulo");
            if (request.Valor <= 0) throw new ValorException("Valor deve ser maior que zero");
            if (request.DataResgate > DateTime.MaxValue.AddYears(-7899) || request.DataResgate < DateTime.MinValue.AddYears(1949)) throw new DataResgateException();

            var resgateRequest = new Resgate(request);
            var investimento = await _investimentosService.ConsultarInvestimento(resgateRequest.IdInvestimento);
            var resgate = await _resgateRepository.RealizarResgate(resgateRequest);
            var response = new RealizarResgateViewModel.Response() { Id = resgate.Id, Valor = resgate.Valor, DataResgate = resgate.DataResgate, MotivoResgate = resgate.MotivoResgate, Investimento = investimento, IdCliente = resgate.IdCliente };
            return response;
        }
    }
}
