using System;
using NFinance.Domain;
using System.Threading.Tasks;
using NFinance.Application.Interfaces;
using NFinance.Domain.Interfaces.Services;
using NFinance.Application.ViewModel.InvestimentosViewModel;

namespace NFinance.Application
{
    public class InvestimentoApp : IInvestimentoApp
    {
        private readonly IInvestimentoService _investimentoService;

        public InvestimentoApp(IInvestimentoService investimentoService)
        {
            _investimentoService = investimentoService;
        }

        public async Task<AtualizarInvestimentoViewModel.Response> AtualizarInvestimento(Guid idInvestimento, AtualizarInvestimentoViewModel.Request request)
        {
            var investimentoDadosAtualizados = new Investimento(idInvestimento, request.IdCliente,request.NomeInvestimento,request.Valor,request.DataAplicacao);
            var investimentoAtualizado = await _investimentoService.AtualizarInvestimento(investimentoDadosAtualizados);
            var resposta = new AtualizarInvestimentoViewModel.Response(investimentoAtualizado);
            return resposta;
        }

        public async Task<ConsultarInvestimentoViewModel.Response> ConsultarInvestimento(Guid idInvestimento)
        {
            var investimento = await _investimentoService.ConsultarInvestimento(idInvestimento);
            var resposta = new ConsultarInvestimentoViewModel.Response(investimento);
            return resposta;
        }

        public async Task<ConsultarInvestimentosViewModel.Response> ConsultarInvestimentos(Guid idCliente)
        {
            var investimentos = await _investimentoService.ConsultarInvestimentos(idCliente);
            var resposta = new ConsultarInvestimentosViewModel.Response(investimentos);
            return resposta;
        }

        public async Task<RealizarInvestimentoViewModel.Response> RealizarInvestimento(RealizarInvestimentoViewModel.Request request)
        {
            var investimento = new Investimento(request.IdCliente, request.NomeInvestimento, request.Valor, request.DataAplicacao);
            var investimentoRealizado = await _investimentoService.AtualizarInvestimento(investimento);
            var resposta = new RealizarInvestimentoViewModel.Response(investimentoRealizado);
            return resposta;
        }
    }
}
