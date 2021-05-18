using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Exceptions.Investimento;

namespace NFinance.Domain.Services
{
    public class InvestimentoService : IInvestimentoService
    {
        private readonly IInvestimentoRepository _investimentosRepository;

        public InvestimentoService(IInvestimentoRepository investimentosRepository)
        {
            _investimentosRepository = investimentosRepository;
        }

        public async Task<Investimento> AtualizarInvestimento(Investimento request)
        {
            if (Guid.Empty.Equals(request.Id)) throw new IdException("Id investimento invalido");
            if (Guid.Empty.Equals(request.IdCliente)) throw new IdException("Id cliente invalido");
            if (string.IsNullOrWhiteSpace(request.NomeInvestimento)) throw new NomeInvestimentoException("Nome nao pode ser nulo,vazio ou em branco");
            if (request.Valor <= 0) throw new ValorInvestimentoException("Valor nao pode ser menor que zero");
            if (request.DataAplicacao > DateTime.MaxValue.AddYears(-7899) || request.DataAplicacao < DateTime.MinValue.AddYears(1949)) throw new DataInvestimentoException();

            return await _investimentosRepository.AtualizarInvestimento(request);
        }

        public async Task<Investimento> ConsultarInvestimento(Guid id)
        {
            if (Guid.Empty.Equals(id)) throw new IdException("Id investimento invalido");
            
            return await _investimentosRepository.ConsultarInvestimento(id);
        }

        public async Task<List<Investimento>> ConsultarInvestimentos(Guid idCliente)
        {
            if (Guid.Empty.Equals(idCliente)) throw new IdException("Id investimento invalido");

            return await _investimentosRepository.ConsultarInvestimentos(idCliente);
        }

        public async Task<Investimento> RealizarInvestimento(Investimento request)
        {
            if (Guid.Empty.Equals(request.IdCliente)) throw new IdException("Id cliente invalido");
            if (string.IsNullOrWhiteSpace(request.NomeInvestimento)) throw new NomeInvestimentoException("Nome nao pode ser nulo,vazio ou em branco");
            if (request.Valor <= 0) throw new ValorInvestimentoException("Valor nao pode ser menor que zero");
            if (request.DataAplicacao > DateTime.MaxValue.AddYears(-7899) || request.DataAplicacao < DateTime.MinValue.AddYears(1949)) throw new DataInvestimentoException();

            return await _investimentosRepository.RealizarInvestimento(request);
        }
    }
}
