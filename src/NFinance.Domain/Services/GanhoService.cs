using System;
using System.Threading.Tasks;
using NFinance.Domain.Exceptions;
using System.Collections.Generic;
using NFinance.Domain.Exceptions.Ganho;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Interfaces.Repository;

namespace NFinance.Domain.Services
{
    public class GanhoService : IGanhoService
    {
        private readonly IGanhoRepository _ganhoRepository;

        public GanhoService(IGanhoRepository ganhoRepository)
        {
            _ganhoRepository = ganhoRepository;
        }

        public async Task<Ganho> CadastrarGanho(Ganho request)
        {
            if (Guid.Empty.Equals(request.IdCliente)) throw new IdException("ID cliente invalido");
            if (string.IsNullOrWhiteSpace(request.NomeGanho)) throw new NomeGanhoException("Nome nao deve ser vazio,branco ou nulo");
            if (request.Valor <= 0) throw new ValorGanhoException("Valor deve ser maior que zero");
            if (request.DataDoGanho > DateTime.MaxValue.AddYears(-7899) || request.DataDoGanho < DateTime.MinValue.AddYears(1949)) throw new DataGanhoException();

            return await _ganhoRepository.CadastrarGanho(request);
        }

        public async Task<Ganho> AtualizarGanho(Ganho request)
        {
            if (Guid.Empty.Equals(request.Id)) throw new IdException("Id invalido");
            if (Guid.Empty.Equals(request.IdCliente)) throw new IdException("Id cliente invalido");
            if (string.IsNullOrWhiteSpace(request.NomeGanho)) throw new NomeGanhoException("Nome nao deve ser vazio,branco ou nulo");
            if (request.Valor <= 0) throw new ValorGanhoException("Valor deve ser maior que zero");
            if (request.DataDoGanho > DateTime.MaxValue.AddYears(-7899) || request.DataDoGanho < DateTime.MinValue.AddYears(1949)) throw new DataGanhoException();

            return await _ganhoRepository.AtualizarGanho(request);
        }

        public async Task<Ganho> ConsultarGanho(Guid id)
        {
            if (Guid.Empty.Equals(id)) throw new IdException("Id invalido");

            return await _ganhoRepository.ConsultarGanho(id);
        }

        public async Task<List<Ganho>> ConsultarGanhos(Guid idCliente)
        {
            if (Guid.Empty.Equals(idCliente)) throw new IdException("Id cliente invalido");

            return await _ganhoRepository.ConsultarGanhos(idCliente);
        }

        public async Task<bool> ExcluirGanho(Ganho request)
        {
            if (Guid.Empty.Equals(request.Id)) throw new IdException("Id invalido");
            if (Guid.Empty.Equals(request.IdCliente)) throw new IdException("Id cliente invalido");

            return await _ganhoRepository.ExcluirGanho(request.Id);
        }
    }
}