using NFinance.Domain.Exceptions;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.ViewModel.GanhoViewModel;
using NFinance.Domain.ViewModel.TelaInicialViewModel;
using NFinance.Model.GastosViewModel;
using NFinance.Model.InvestimentosViewModel;
using NFinance.Model.ResgatesViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Services
{
    public class TelaInicialService : ITelaInicialService
    {
        private readonly IGanhoRepository _ganhoRepository;
        private readonly IInvestimentoRepository _investimentoRepository;
        private readonly IGastoRepository _gastoRepository;
        private readonly IResgateRepository _resgateRepository;
        private readonly IClienteService _clienteService;

        public TelaInicialService(IGanhoRepository ganhoRepository, IInvestimentoRepository investimentoRepository, IGastoRepository gastoRepository, IResgateRepository resgateRepository, IClienteService clienteService)
        {
            _ganhoRepository = ganhoRepository;
            _investimentoRepository = investimentoRepository;
            _gastoRepository = gastoRepository;
            _resgateRepository = resgateRepository;
            _clienteService = clienteService;
        }

        public async Task<TelaInicialViewModel> TelaInicial(Guid idCliente)
        {
            if (Guid.Empty.Equals(idCliente)) throw new IdException("ID gasto invalido");

            var cliente = await _clienteService.ConsultarCliente(idCliente);
            var ganhoMensal = await GanhoMensal(idCliente);
            var gastoMensal = await GastoMensal(idCliente);
            var investimentoMensal = await InvestimentoMensal(idCliente);
            var resgateMensal = await ResgateMensal(idCliente);

            var resumoMensal = ganhoMensal.SaldoMensal - gastoMensal.SaldoMensal;

            var response = new TelaInicialViewModel(cliente, ganhoMensal, gastoMensal, investimentoMensal, resgateMensal, resumoMensal);

            return response;
        }

        public async Task<GanhoMensalViewModel> GanhoMensal(Guid idCliente)
        {

            var ganhos = await _ganhoRepository.ConsultarGanhos(idCliente);
            var listGanho = new List<GanhoViewModel>();
            foreach (var ganho in ganhos)
                if (ganho.DataDoGanho.Month.Equals(DateTime.Today.Month) && ganho.Recorrente)
                {
                    var vm = new GanhoViewModel(ganho);
                    listGanho.Add(vm);
                    ganho.DataDoGanho.AddMonths(1);
                }
                else
                {
                    if (ganho.DataDoGanho.Month.Equals(DateTime.Today.Month) && !ganho.Recorrente)
                    {
                        var vm = new GanhoViewModel(ganho);
                        listGanho.Add(vm);
                    }
                }

            var respose = new GanhoMensalViewModel(listGanho);

            return respose;
        }

        public async Task<GastoMensalViewModel> GastoMensal(Guid idCliente)
        {
            var gastos = await _gastoRepository.ConsultarGastos(idCliente);
            var listGastos = new List<GastoViewModel.Response>();

            foreach (var gasto in gastos)
                if (gasto.DataDoGasto.Month.Equals(DateTime.Today.Month))
                    if (gasto.DataDoGasto.AddMonths(gasto.QuantidadeParcelas).Month >= DateTime.Today.Month)
                    {
                        var vm = new GastoViewModel.Response(gasto);
                        listGastos.Add(vm);
                        var gastoAtt = new Gasto { Id = gasto.Id, IdCliente = gasto.IdCliente, NomeGasto = gasto.NomeGasto, Valor = gasto.Valor, DataDoGasto = gasto.DataDoGasto, QuantidadeParcelas = gasto.QuantidadeParcelas - 1 };
                        await _gastoRepository.AtualizarGasto(gasto.Id, gastoAtt);
                    }

            var response = new GastoMensalViewModel(listGastos);

            return response;
        }

        public async Task<InvestimentoMensalViewModel> InvestimentoMensal(Guid idCliente)
        {
            var investimentos = await _investimentoRepository.ConsultarInvestimentos(idCliente);
            var listInvestimentos = new List<InvestimentoViewModel.Response>();

            foreach (var investimento in investimentos)
                if (investimento.DataAplicacao.Month.Equals(DateTime.Today.Month))
                {
                    var vm = new InvestimentoViewModel.Response(investimento);
                    listInvestimentos.Add(vm);
                }

            var response = new InvestimentoMensalViewModel(listInvestimentos);

            return response;
        }

        public async Task<ResgateMensalViewModel> ResgateMensal(Guid idCliente)
        {
            var resgates = await _resgateRepository.ConsultarResgates(idCliente);
            var listResgates = new List<ResgateViewModel.Response>();

            foreach (var resgate in resgates)
                if (resgate.DataResgate.Month.Equals(DateTime.Today.Month))
                {
                    var vm = new ResgateViewModel.Response(resgate);
                    listResgates.Add(vm);
                }

            var response = new ResgateMensalViewModel(listResgates);

            return response;
        }


    }
}
