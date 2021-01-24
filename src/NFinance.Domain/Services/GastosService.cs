using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Interfaces.Services;
using NFinance.Model.ClientesViewModel;
using NFinance.Model.GastosViewModel;
using System;
using System.Threading.Tasks;

namespace NFinance.Domain.Services
{
    public class GastosService : IGastosService
    {
        private readonly IGastosRepository _gastosRepository;
        private readonly IClienteRepository _clienteRepository;
        public GastosService(IGastosRepository gastosRepository, IClienteRepository clienteRepository)
        {
            _gastosRepository = gastosRepository;
            _clienteRepository = clienteRepository;
        }

        public async Task<AtualizarGastoViewModel.Response> AtualizarGasto(Guid id, AtualizarGastoViewModel.Request request)
        {
            var gasto = new Gastos(id,request);
            var cliente = await _clienteRepository.ConsultarCliente(request.IdCliente);
            var atualizado = await _gastosRepository.AtualizarGasto(id, gasto);
            var response = new AtualizarGastoViewModel.Response() {Id = atualizado.Id, Cliente = new ClienteViewModel.Response() {Id = cliente.Id,Nome = cliente.Nome } , Nome = atualizado.Nome,DataDoGasto = atualizado.DataDoGasto, QuantidadeParcelas = atualizado.QuantidadeParcelas,ValorTotal = atualizado.ValorTotal };
            return response;
        }

        public async Task<CadastrarGastoViewModel.Response> CadastrarGasto(CadastrarGastoViewModel.Request request)
        {
            var gasto = new Gastos(request);
            var cliente = await _clienteRepository.ConsultarCliente(request.IdCliente);
            var cadastro = await _gastosRepository.CadastrarGasto(gasto);
            var response = new CadastrarGastoViewModel.Response() { Id = cadastro.Id, Cliente = new ClienteViewModel.Response() { Id = cliente.Id, Nome = cliente.Nome }, Nome = cadastro.Nome, DataDoGasto = cadastro.DataDoGasto, QuantidadeParcelas = cadastro.QuantidadeParcelas, ValorTotal = cadastro.ValorTotal };
            return response;
        }

        public async Task<ConsultarGastoViewModel.Response> ConsultarGasto(Guid id)
        {
            var gasto = await _gastosRepository.ConsultarGasto(id);
            var cliente = await _clienteRepository.ConsultarCliente(gasto.IdCliente);
            var response = new ConsultarGastoViewModel.Response() { Id = gasto.Id, Cliente = new ClienteViewModel.Response() { Id = cliente.Id, Nome = cliente.Nome }, Nome = gasto.Nome, DataDoGasto = gasto.DataDoGasto, QuantidadeParcelas = gasto.QuantidadeParcelas, ValorTotal = gasto.ValorTotal };
            return response;
        }

        public async Task<ExcluirGastoViewModel.Response> ExcluirGasto(ExcluirGastoViewModel.Request request)
        {
            var excluir = await _gastosRepository.ExcluirGasto(request.IdGasto);
            if(excluir == true)
                return new ExcluirGastoViewModel.Response() { StatusCode = 200, DataExclusao = DateTime.UtcNow, Mensagem = "Excluido Com Sucesso" };
             else
                return new ExcluirGastoViewModel.Response() { StatusCode = 400, DataExclusao = DateTime.UtcNow, Mensagem = "Excluido Com Sucesso" };
        }

        public async Task<ListarGastosViewModel.Response> ListarGastos()
        {
            var listaGastos = await _gastosRepository.ListarGastos();
            var response = new ListarGastosViewModel.Response(listaGastos);
            return response;
        }
    }
}
