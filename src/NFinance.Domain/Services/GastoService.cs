using NFinance.Domain.Exceptions;
using NFinance.Domain.Exceptions.Gasto;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Interfaces.Services;
using NFinance.Model.GastosViewModel;
using System;
using System.Threading.Tasks;
using NFinance.Domain.ViewModel.ClientesViewModel;

namespace NFinance.Domain.Services
{
    public class GastoService : IGastoService
    {
        private readonly IGastoRepository _gastosRepository;
        private readonly IClienteService _clienteService;
        public GastoService(IGastoRepository gastosRepository, IClienteService clienteService)
        {
            _gastosRepository = gastosRepository;
            _clienteService = clienteService;
        }

        public async Task<AtualizarGastoViewModel.Response> AtualizarGasto(Guid id, AtualizarGastoViewModel.Request request)
        {
            if (Guid.Empty.Equals(id)) throw new IdException("ID gasto invalido");
            if (Guid.Empty.Equals(request.IdCliente)) throw new IdException("ID cliente invalido");
            if (string.IsNullOrWhiteSpace(request.NomeGasto)) throw new NomeGastoException("Nome nao deve ser vazio,branco ou nulo");
            if (request.Valor <= 0) throw new ValorGastoException("Valor deve ser maior que zero");
            if (request.QuantidadeParcelas < 0 || request.QuantidadeParcelas >= 1000) throw new QuantidadeParcelaException("Valor deve ser maior que zero e menor que mil");
            if (request.DataDoGasto > DateTime.MaxValue.AddYears(-7899) || request.DataDoGasto < DateTime.MinValue.AddYears(1949)) throw new DataGastoException();

            var gasto = new Gasto(id,request);
            var cliente = await _clienteService.ConsultarCliente(request.IdCliente);
            var atualizado = await _gastosRepository.AtualizarGasto(id, gasto);
            var response = new AtualizarGastoViewModel.Response {Id = atualizado.Id, Cliente = new ClienteViewModel.Response() {Id = cliente.Id,Nome = cliente.Nome } , NomeGasto = atualizado.NomeGasto,DataDoGasto = atualizado.DataDoGasto, QuantidadeParcelas = atualizado.QuantidadeParcelas,Valor = atualizado.Valor };
            return response;
        }

        public async Task<CadastrarGastoViewModel.Response> CadastrarGasto(CadastrarGastoViewModel.Request request)
        {
            if (Guid.Empty.Equals(request.IdCliente)) throw new IdException("ID cliente invalido");
            if (string.IsNullOrWhiteSpace(request.NomeGasto)) throw new NomeGastoException("Nome nao deve ser vazio,branco ou nulo");
            if (request.Valor <= 0) throw new ValorGastoException("Valor deve ser maior que zero");
            if (request.QuantidadeParcelas <= 0 || request.QuantidadeParcelas >= 1000) throw new QuantidadeParcelaException("Valor deve ser maior que zero e menor que mil");
            if (request.DataDoGasto > DateTime.MaxValue.AddYears(-7899) || request.DataDoGasto < DateTime.MinValue.AddYears(1949)) throw new DataGastoException();

            var gasto = new Gasto(request);
            var cliente = await _clienteService.ConsultarCliente(request.IdCliente);
            var cadastro = await _gastosRepository.CadastrarGasto(gasto);
            var response = new CadastrarGastoViewModel.Response { Id = cadastro.Id, Cliente = new ClienteViewModel.Response() { Id = cliente.Id, Nome = cliente.Nome }, NomeGasto = cadastro.NomeGasto, DataDoGasto = cadastro.DataDoGasto, QuantidadeParcelas = cadastro.QuantidadeParcelas, Valor = cadastro.Valor };
            return response;
        }

        public async Task<ConsultarGastoViewModel.Response> ConsultarGasto(Guid id)
        {
            if (Guid.Empty.Equals(id)) throw new IdException("ID gasto invalido");

            var gasto = await _gastosRepository.ConsultarGasto(id);
            var cliente = await _clienteService.ConsultarCliente(gasto.IdCliente);
            var response = new ConsultarGastoViewModel.Response { Id = gasto.Id, Cliente = new ClienteViewModel.Response() { Id = cliente.Id, Nome = cliente.Nome }, NomeGasto = gasto.NomeGasto, DataDoGasto = gasto.DataDoGasto, QuantidadeParcelas = gasto.QuantidadeParcelas, Valor = gasto.Valor };
            return response;
        }

        public async Task<ConsultarGastosViewModel.Response> ConsultarGastos(Guid idCliente)
        {
            if (Guid.Empty.Equals(idCliente)) throw new IdException("ID cliente invalido");

            var consultarInvestimentos = await _gastosRepository.ConsultarGastos(idCliente);
            var response = new ConsultarGastosViewModel.Response(consultarInvestimentos);
            return response;
        }

        public async Task<ExcluirGastoViewModel.Response> ExcluirGasto(ExcluirGastoViewModel.Request request)
        {
            if (Guid.Empty.Equals(request.IdGasto)) throw new IdException("ID gasto invalido");
            if (Guid.Empty.Equals(request.IdCliente)) throw new IdException("ID gasto invalido");
            if (string.IsNullOrWhiteSpace(request.MotivoExclusao)) throw new NomeGastoException("Motivo exclusao nao pode ser vazio,branco ou nulo");

            var excluir = await _gastosRepository.ExcluirGasto(request.IdGasto);
            if(excluir)
                return new ExcluirGastoViewModel.Response { StatusCode = 200, DataExclusao = DateTime.UtcNow, Mensagem = "Excluido Com Sucesso" };
             else
                return new ExcluirGastoViewModel.Response { StatusCode = 400, DataExclusao = DateTime.UtcNow, Mensagem = "Ocorreu um erro ao Excluir" };
        }
    }
}
