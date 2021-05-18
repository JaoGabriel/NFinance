using System;
using NFinance.Domain;
using System.Threading.Tasks;
using NFinance.Application.Interfaces;
using NFinance.Domain.Interfaces.Services;
using NFinance.Application.ViewModel.GastosViewModel;

namespace NFinance.Application
{
    public class GastoApp : IGastoApp
    {
        private readonly IGastoService _gastoService;

        public GastoApp(IGastoService gastoService)
        {
            _gastoService = gastoService;
        }

        public async Task<AtualizarGastoViewModel.Response> AtualizarGasto(Guid idGasto, AtualizarGastoViewModel.Request request)
        {
            var gastoDadosAtualizados = new Gasto(idGasto,request.IdCliente,request.NomeGasto,request.Valor,request.QuantidadeParcelas,request.DataDoGasto);
            var gastoAtualizado = await _gastoService.AtualizarGasto(gastoDadosAtualizados);
            var resposta = new AtualizarGastoViewModel.Response(gastoAtualizado);
            return resposta;
        }

        public async Task<CadastrarGastoViewModel.Response> CadastrarGasto(CadastrarGastoViewModel.Request request)
        {
            var gasto = new Gasto(request.IdCliente, request.NomeGasto, request.Valor, request.QuantidadeParcelas, request.DataDoGasto);
            var gastoCadastrado = await _gastoService.CadastrarGasto(gasto);
            var resposta = new CadastrarGastoViewModel.Response(gastoCadastrado);
            return resposta;
        }

        public async Task<ConsultarGastoViewModel.Response> ConsultarGasto(Guid idGasto)
        {
            var gasto = await _gastoService.ConsultarGasto(idGasto);
            var resposta = new ConsultarGastoViewModel.Response(gasto);
            return resposta;
        }

        public async Task<ConsultarGastosViewModel.Response> ConsultarGastos(Guid idCliente)
        {
            var gastos = await _gastoService.ConsultarGastos(idCliente);
            var resposta = new ConsultarGastosViewModel.Response(gastos);
            return resposta;
        }

        public async Task<ExcluirGastoViewModel.Response> ExcluirGasto(ExcluirGastoViewModel.Request request)
        {
            var gastoAExcluir = await _gastoService.ConsultarGasto(request.IdGasto);
            //TODO: Verificar para a inclusao da coluna motivo exclusao na tabela
            var gastoExcluido = await _gastoService.ExcluirGasto(gastoAExcluir);
            var resposta = new ExcluirGastoViewModel.Response(gastoExcluido);
            return resposta;
        }
    }
}
