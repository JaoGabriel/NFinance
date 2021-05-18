using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Gasto;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Interfaces.Repository;

namespace NFinance.Domain.Services
{
    public class GastoService : IGastoService
    {
        private readonly IGastoRepository _gastosRepository;
        public GastoService(IGastoRepository gastosRepository)
        {
            _gastosRepository = gastosRepository;
        }

        public async Task<Gasto> AtualizarGasto(Gasto request)
        {
            if (Guid.Empty.Equals(request.Id)) throw new IdException("ID gasto invalido");
            if (Guid.Empty.Equals(request.IdCliente)) throw new IdException("ID cliente invalido");
            if (string.IsNullOrWhiteSpace(request.NomeGasto)) throw new NomeGastoException("Nome nao deve ser vazio,branco ou nulo");
            if (request.Valor <= 0) throw new ValorGastoException("Valor deve ser maior que zero");
            if (request.QuantidadeParcelas <= 0 || request.QuantidadeParcelas >= 1000) throw new QuantidadeParcelaException("Valor deve ser maior que zero e menor que mil");
            if (request.DataDoGasto > DateTime.MaxValue.AddYears(-7899) || request.DataDoGasto < DateTime.MinValue.AddYears(1949)) throw new DataGastoException();

            return await _gastosRepository.AtualizarGasto(request);
        }

        public async Task<Gasto> CadastrarGasto(Gasto request)
        {
            if (Guid.Empty.Equals(request.IdCliente)) throw new IdException("ID cliente invalido");
            if (string.IsNullOrWhiteSpace(request.NomeGasto)) throw new NomeGastoException("Nome nao deve ser vazio,branco ou nulo");
            if (request.Valor <= 0) throw new ValorGastoException("Valor deve ser maior que zero");
            if (request.QuantidadeParcelas <= 0 || request.QuantidadeParcelas >= 1000) throw new QuantidadeParcelaException("Valor deve ser maior que zero e menor que mil");
            if (request.DataDoGasto > DateTime.MaxValue.AddYears(-7899) || request.DataDoGasto < DateTime.MinValue.AddYears(1949)) throw new DataGastoException();

            return await _gastosRepository.CadastrarGasto(request);
        }

        public async Task<Gasto> ConsultarGasto(Guid id)
        {
            if (Guid.Empty.Equals(id)) throw new IdException("Id gasto invalido");

            return await _gastosRepository.ConsultarGasto(id);
        }

        public async Task<List<Gasto>> ConsultarGastos(Guid idCliente)
        {
            if (Guid.Empty.Equals(idCliente)) throw new IdException("Id cliente invalido");

            return await _gastosRepository.ConsultarGastos(idCliente);
        }

        public async Task<bool> ExcluirGasto(Gasto request)
        {
            if (Guid.Empty.Equals(request.Id)) throw new IdException("Id gasto invalido");
            if (Guid.Empty.Equals(request.IdCliente)) throw new IdException("Id gasto invalido");

            return await _gastosRepository.ExcluirGasto(request.Id);
        }
    }
}
