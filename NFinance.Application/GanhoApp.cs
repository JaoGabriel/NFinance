using System;
using NFinance.Domain;
using System.Threading.Tasks;
using NFinance.Application.Interfaces;
using NFinance.Domain.Interfaces.Services;
using NFinance.Application.ViewModel.GanhoViewModel;

namespace NFinance.Application
{
    public class GanhoApp : IGanhoApp
    {
        private readonly IGanhoService _ganhoService;

        public GanhoApp(IGanhoService ganhoService)
        {
            _ganhoService = ganhoService;
        }

        public async Task<AtualizarGanhoViewModel.Response> AtualizarGanho(Guid idGanho, AtualizarGanhoViewModel.Request request)
        {
            var ganhoDadosAtualizados = new Ganho(idGanho,request.IdCliente,request.NomeGanho,request.Valor,request.Recorrente,request.DataDoGanho);
            var ganhoAtualizado = await _ganhoService.AtualizarGanho(ganhoDadosAtualizados);
            var resposta = new AtualizarGanhoViewModel.Response(ganhoAtualizado);
            return resposta;        
        }

        public async Task<CadastrarGanhoViewModel.Response> CadastrarGanho(CadastrarGanhoViewModel.Request request)
        {
            var ganho = new Ganho(request.IdCliente, request.NomeGanho, request.Valor, request.Recorrente, request.DataDoGanho);
            var ganhoCadastrado = await _ganhoService.CadastrarGanho(ganho);
            var resposta = new CadastrarGanhoViewModel.Response(ganhoCadastrado);
            return resposta;
        }

        public async Task<ConsultarGanhoViewModel.Response> ConsultarGanho(Guid idGanho)
        {
            var ganhoConsultado = await _ganhoService.ConsultarGanho(idGanho);
            var resposta = new ConsultarGanhoViewModel.Response(ganhoConsultado);
            return resposta;
        }

        public async Task<ConsultarGanhosViewModel.Response> ConsultarGanhos(Guid idCliente)
        {
            var ganhosConsultados = await _ganhoService.ConsultarGanhos(idCliente);
            var resposta = new ConsultarGanhosViewModel.Response(ganhosConsultados);
            return resposta;
        }

        public async Task<ExcluirGanhoViewModel.Response> ExcluirGanho(ExcluirGanhoViewModel.Request request)
        {
            var ganhoAExcluir = await _ganhoService.ConsultarGanho(request.IdGanho);
            //TODO: Verificar para a inclusao da coluna motivo exclusao na tabela
            var ganhoCadastrado = await _ganhoService.ExcluirGanho(ganhoAExcluir);
            var resposta = new ExcluirGanhoViewModel.Response(ganhoCadastrado);
            return resposta;
        }
    }
}
