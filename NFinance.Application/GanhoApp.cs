using System;
using NFinance.Domain;
using System.Threading.Tasks;
using NFinance.Application.Interfaces;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Application.ViewModel.GanhoViewModel;
using NFinance.Domain.Exceptions;

namespace NFinance.Application
{
    public class GanhoApp : IGanhoApp
    {
        private readonly IGanhoRepository _ganhoRepository;

        public GanhoApp(IGanhoRepository ganhoRepository)
        {
            _ganhoRepository = ganhoRepository;
        }

        public async Task<AtualizarGanhoViewModel.Response> AtualizarGanho(Guid idGanho, AtualizarGanhoViewModel.Request request)
        {
            var ganhoDadosAtualizados = new Ganho(idGanho,request.IdCliente,request.NomeGanho,request.Valor,request.Recorrente,request.DataDoGanho);
            var ganhoAtualizado = await _ganhoRepository.AtualizarGanho(ganhoDadosAtualizados);
            var resposta = new AtualizarGanhoViewModel.Response(ganhoAtualizado);
            return resposta;        
        }

        public async Task<CadastrarGanhoViewModel.Response> CadastrarGanho(CadastrarGanhoViewModel.Request request)
        {
            var ganho = new Ganho(request.IdCliente, request.NomeGanho, request.Valor, request.Recorrente, request.DataDoGanho);
            var ganhoCadastrado = await _ganhoRepository.CadastrarGanho(ganho);
            var resposta = new CadastrarGanhoViewModel.Response(ganhoCadastrado);
            return resposta;
        }

        public async Task<ConsultarGanhoViewModel.Response> ConsultarGanho(Guid idGanho)
        {
            ValidaId(idGanho);
            var ganhoConsultado = await _ganhoRepository.ConsultarGanho(idGanho);
            var resposta = new ConsultarGanhoViewModel.Response(ganhoConsultado);
            return resposta;
        }

        public async Task<ConsultarGanhosViewModel.Response> ConsultarGanhos(Guid idCliente)
        {
            ValidaId(idCliente);
            var ganhosConsultados = await _ganhoRepository.ConsultarGanhos(idCliente);
            var resposta = new ConsultarGanhosViewModel.Response(ganhosConsultados);
            return resposta;
        }

        public async Task<ExcluirGanhoViewModel.Response> ExcluirGanho(ExcluirGanhoViewModel.Request request)
        {
            //TODO: Verificar para a inclusao da coluna motivo exclusao na tabela
            var ganhoCadastrado = await _ganhoRepository.ExcluirGanho(request.IdGanho);
            var resposta = new ExcluirGanhoViewModel.Response(ganhoCadastrado);
            return resposta;
        }

        private static void ValidaId(Guid id)
        {
            if (Guid.Empty.Equals(id)) 
                throw new IdException();
        }
    }
}
