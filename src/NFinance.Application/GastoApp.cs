using System;
using NFinance.Domain;
using System.Threading.Tasks;
using NFinance.Application.Interfaces;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Application.ViewModel.GastosViewModel;

namespace NFinance.Application
{
    public class GastoApp : IGastoApp
    {
        private readonly IGastoRepository _gastoRepository;

        public GastoApp(IGastoRepository gastoRepository)
        {
            _gastoRepository = gastoRepository;
        }

        public async Task<AtualizarGastoViewModel.Response> AtualizarGasto(Guid idGasto, AtualizarGastoViewModel.Request request)
        {
            var gasto = await _gastoRepository.ConsultarGasto(idGasto);
            gasto.AtualizaGasto(request.NomeGasto,request.Valor,request.QuantidadeParcelas,request.DataDoGasto);
            var gastoAtualizado = await _gastoRepository.AtualizarGasto(gasto);
            var resposta = new AtualizarGastoViewModel.Response(gastoAtualizado);
            return resposta;
        }

        public async Task<CadastrarGastoViewModel.Response> CadastrarGasto(CadastrarGastoViewModel.Request request)
        {
            var gasto = new Gasto(request.IdCliente, request.NomeGasto, request.Valor, request.QuantidadeParcelas, request.DataDoGasto);
            var gastoCadastrado = await _gastoRepository.CadastrarGasto(gasto);
            var resposta = new CadastrarGastoViewModel.Response(gastoCadastrado);
            return resposta;
        }

        public async Task<ConsultarGastoViewModel.Response> ConsultarGasto(Guid idGasto)
        {
            var gasto = await _gastoRepository.ConsultarGasto(idGasto);
            var resposta = new ConsultarGastoViewModel.Response(gasto);
            return resposta;
        }

        public async Task<ConsultarGastosViewModel.Response> ConsultarGastos(Guid idCliente)
        {
            var gastos = await _gastoRepository.ConsultarGastos(idCliente);
            var resposta = new ConsultarGastosViewModel.Response(gastos);
            return resposta;
        }

        public async Task<ExcluirGastoViewModel.Response> ExcluirGasto(ExcluirGastoViewModel.Request request)
        {
            //TODO: Verificar para a inclusao da coluna motivo exclusao na tabela
            var gastoExcluido = await _gastoRepository.ExcluirGasto(request.IdGasto);
            var resposta = new ExcluirGastoViewModel.Response(gastoExcluido);
            return resposta;
        }
    }
}
