using System;
using NFinance.Domain;
using System.Threading.Tasks;
using NFinance.Application.Interfaces;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Application.ViewModel.InvestimentosViewModel;

namespace NFinance.Application
{
    public class InvestimentoApp : IInvestimentoApp
    {
        private readonly IInvestimentoRepository _investimentoRepository;

        public InvestimentoApp(IInvestimentoRepository investimentoRepository)
        {
            _investimentoRepository = investimentoRepository;
        }

        public async Task<AtualizarInvestimentoViewModel.Response> AtualizarInvestimento(Guid idInvestimento, AtualizarInvestimentoViewModel.Request request)
        {
            var investimento = await _investimentoRepository.ConsultarInvestimento(idInvestimento);
            investimento.AtualizaInvestimento(request.NomeInvestimento,request.Valor,request.DataAplicacao);
            var investimentoAtualizado = await _investimentoRepository.AtualizarInvestimento(investimento);
            var resposta = new AtualizarInvestimentoViewModel.Response(investimentoAtualizado);
            return resposta;
        }

        public async Task<ConsultarInvestimentoViewModel.Response> ConsultarInvestimento(Guid idInvestimento)
        {
            var investimento = await _investimentoRepository.ConsultarInvestimento(idInvestimento);
            var resposta = new ConsultarInvestimentoViewModel.Response(investimento);
            return resposta;
        }

        public async Task<ConsultarInvestimentosViewModel.Response> ConsultarInvestimentos(Guid idCliente)
        {
            var investimentos = await _investimentoRepository.ConsultarInvestimentos(idCliente);
            var resposta = new ConsultarInvestimentosViewModel.Response(investimentos);
            return resposta;
        }

        public async Task<RealizarInvestimentoViewModel.Response> RealizarInvestimento(RealizarInvestimentoViewModel.Request request)
        {
            var investimento = new Investimento(request.IdCliente, request.NomeInvestimento, request.Valor, request.DataAplicacao);
            var investimentoRealizado = await _investimentoRepository.AtualizarInvestimento(investimento);
            var resposta = new RealizarInvestimentoViewModel.Response(investimentoRealizado);
            return resposta;
        }
    }
}
