using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Resgate;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Interfaces.Repository;

namespace NFinance.Domain.Services
{
    public class ResgateService : IResgateService
    {
        private readonly IResgateRepository _resgateRepository;

        public ResgateService(IResgateRepository resgateRepository)
        {
            _resgateRepository = resgateRepository;
        }

        public async Task<Resgate> ConsultarResgate(Guid id)
        {
            if (Guid.Empty.Equals(id)) throw new IdException("Id resgate invalido");

            return await _resgateRepository.ConsultarResgate(id);
        }

        public async Task<List<Resgate>> ConsultarResgates(Guid idCliente)
        {
            if (Guid.Empty.Equals(idCliente)) throw new IdException("Id cliente invalido");

            return await _resgateRepository.ConsultarResgates(idCliente);
        }

        public async Task<Resgate> RealizarResgate(Resgate request)
        {
            if (Guid.Empty.Equals(request.IdCliente)) throw new IdException("Id cliente invalido");
            if (Guid.Empty.Equals(request.IdInvestimento)) throw new IdException("Id investimento invalido");
            if (string.IsNullOrWhiteSpace(request.MotivoResgate)) throw new MotivoResgateException("Motivo nao deve ser branco,vazio ou nulo");
            if (request.Valor <= 0) throw new ValorException("Valor deve ser maior que zero");
            if (request.DataResgate > DateTime.MaxValue.AddYears(-7899) || request.DataResgate < DateTime.MinValue.AddYears(1949)) throw new DataResgateException();

            return await _resgateRepository.RealizarResgate(request);
        }
    }
}
