using System;
using System.Threading.Tasks;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Ganho;
using NFinance.Domain.Exceptions.Gasto;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.ViewModel.GanhoViewModel;

namespace NFinance.Domain.Services
{
    public class GanhoService : IGanhoService
    {
        private readonly IGanhoRepository _ganhoRepository;

        public GanhoService(IGanhoRepository ganhoRepository)
        {
            _ganhoRepository = ganhoRepository;
        }

        public async Task<CadastrarGanhoViewModel.Response> CadastrarGanho(CadastrarGanhoViewModel.Request request)
        {
            if (Guid.Empty.Equals(request.IdCliente)) throw new IdException("ID cliente invalido");
            if (string.IsNullOrWhiteSpace(request.NomeGanho)) throw new NomeGanhoException("Nome nao deve ser vazio,branco ou nulo");
            if (request.Valor <= 0) throw new ValorGanhoException("Valor deve ser maior que zero");

            var ganho = new Ganho(request);
            var cadastroGanho = await _ganhoRepository.CadastrarGanho(ganho);
            var response = new CadastrarGanhoViewModel.Response(cadastroGanho);
            return response;
        }

        public async Task<AtualizarGanhoViewModel.Response> AtualizarGanho(Guid id,
            AtualizarGanhoViewModel.Request request)
        {
            if (Guid.Empty.Equals(id)) throw new IdException("Id invalido");
            if (Guid.Empty.Equals(request.IdCliente)) throw new IdException("Id cliente invalido");
            if (string.IsNullOrWhiteSpace(request.NomeGanho)) throw new NomeGanhoException("Nome nao deve ser vazio,branco ou nulo");
            if (request.Valor <= 0) throw new ValorGanhoException("Valor deve ser maior que zero");

            var ganho = new Ganho(id,request);
            var atualizarGanho = await _ganhoRepository.AtualizarGanho(id, ganho);
            var response = new AtualizarGanhoViewModel.Response(atualizarGanho);
            return response;
        }

        public async Task<ConsultarGanhoViewModel.Response> ConsultarGanho(Guid id)
        {
            if (Guid.Empty.Equals(id)) throw new IdException("Id invalido");

            var consultarGanho = await _ganhoRepository.ConsultarGanho(id);
            var response = new ConsultarGanhoViewModel.Response(consultarGanho);
            return response;
        }

        public async Task<ConsultarGanhosViewModel.Response> ConsultarGanhos(Guid idCliente)
        {
            if (Guid.Empty.Equals(idCliente)) throw new IdException("Id cliente invalido");

            var consultarGanhos = await _ganhoRepository.ConsultarGanhos(idCliente);
            var response = new ConsultarGanhosViewModel.Response(consultarGanhos);
            return response;
        }

        public async Task<ExcluirGanhoViewModel.Response> ExcluirGanho(ExcluirGanhoViewModel.Request request)
        {
            if (Guid.Empty.Equals(request.IdGanho)) throw new IdException("ID invalido");
            if (Guid.Empty.Equals(request.IdCliente)) throw new IdException("ID cliente invalido");
            if (string.IsNullOrWhiteSpace(request.MotivoExclusao)) throw new NomeGastoException("Motivo exclusao nao pode ser vazio,branco ou nulo");

            var excluir = await _ganhoRepository.ExcluirGanho(request.IdGanho);
            var response = new ExcluirGanhoViewModel.Response(excluir);
            return response;
        }

        public async Task<ListarGanhosViewModel.Response> ListarGanhos()
        {
            var listarGanhos = await _ganhoRepository.ListarGanhos();
            var response = new ListarGanhosViewModel.Response(listarGanhos);
            return response;
        }
    }
}