using System;
using Cronos;
using NFinance.Domain;
using System.Threading.Tasks;
using NFinance.Domain.Exceptions;
using System.Collections.Generic;
using NFinance.Application.Interfaces;
using NFinance.Domain.Interfaces.Services;
using NFinance.Application.ViewModel.GanhoViewModel;
using NFinance.Application.ViewModel.GastosViewModel;
using NFinance.Application.ViewModel.ResgatesViewModel;
using NFinance.Application.ViewModel.TelaInicialViewModel;
using NFinance.Application.ViewModel.InvestimentosViewModel;

namespace NFinance.Application
{
    public class TelaInicialApp : ITelaInicialApp
    {
        private readonly IGanhoService _ganhoService;
        private readonly IInvestimentoService _investimentoService;
        private readonly IGastoService _gastoService;
        private readonly IResgateService _resgateService;
        private readonly IClienteService _clienteService;

        public TelaInicialApp(IGanhoService ganhoService, IInvestimentoService investimentoService, IGastoService gastoService, IResgateService resgateService, IClienteService clienteService)
        {
            _ganhoService = ganhoService;
            _investimentoService = investimentoService;
            _gastoService = gastoService;
            _resgateService = resgateService;
            _clienteService = clienteService;
        }

        public async Task<TelaInicialViewModel> TelaInicial(Guid idCliente)
        {
            if (Guid.Empty.Equals(idCliente)) throw new IdException("Id cliente invalido");

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

            var ganhos = await _ganhoService.ConsultarGanhos(idCliente);
            var listGanho = new List<GanhoViewModel>();
            foreach (var ganho in ganhos)
                if (ValidaGanho(ganho))
                {
                    var vm = new GanhoViewModel(ganho);
                    listGanho.Add(vm);
                }

            var respose = new GanhoMensalViewModel(listGanho);

            return respose;
        }

        public async Task<GastoMensalViewModel> GastoMensal(Guid idCliente)
        {
            var gastos = await _gastoService.ConsultarGastos(idCliente);
            var listGastos = new List<GastoViewModel>();

            foreach (var gasto in gastos)
                if (ValidaGasto(gasto))
                {
                    var vm = new GastoViewModel(gasto);
                    if (gasto.QuantidadeParcelas > 0)
                    {
                        if (ValidaProximoMes(DateTime.Today))
                        {
                            gasto.QuantidadeParcelas -= 1;
                            await _gastoService.AtualizarGasto(gasto);
                        }
                        gasto.Valor /= gasto.QuantidadeParcelas;
                        vm.Valor = gasto.Valor;
                        vm.QuantidadeParcelas = gasto.QuantidadeParcelas;
                    }
                    listGastos.Add(vm);
                }

            var response = new GastoMensalViewModel(listGastos);

            return response;
        }

        public async Task<InvestimentoMensalViewModel> InvestimentoMensal(Guid idCliente)
        {
            var investimentos = await _investimentoService.ConsultarInvestimentos(idCliente);
            var listInvestimentos = new List<InvestimentoViewModel>();

            foreach (var investimento in investimentos)
                if (investimento.DataAplicacao.Month.Equals(DateTime.Today.Month))
                {
                    var vm = new InvestimentoViewModel(investimento);
                    listInvestimentos.Add(vm);
                }

            var response = new InvestimentoMensalViewModel(listInvestimentos);

            return response;
        }

        public async Task<ResgateMensalViewModel> ResgateMensal(Guid idCliente)
        {
            var resgates = await _resgateService.ConsultarResgates(idCliente);
            var listResgates = new List<ResgateViewModel>();

            foreach (var resgate in resgates)
                if (resgate.DataResgate.Month.Equals(DateTime.Today.Month))
                {
                    var vm = new ResgateViewModel(resgate);
                    listResgates.Add(vm);
                }

            var response = new ResgateMensalViewModel(listResgates);

            return response;
        }

        private static bool ValidaProximoMes(DateTime date)
        {
            var cron = CronExpression.Parse("* * 1 * *");
            var proxOcorrencia = cron.GetNextOccurrence(DateTime.UtcNow);

            if (proxOcorrencia?.Month == date.Month)
                return true;
            else
                return false;
        }

        private static bool ValidaGasto(Gasto gasto)
        {
            if (gasto.DataDoGasto.Month > DateTime.Today.Month || gasto.DataDoGasto.Year > DateTime.Today.Year)
                return false;

            if (gasto.QuantidadeParcelas == 1)
                gasto.QuantidadeParcelas -= 1;

            var dataMax = gasto.DataDoGasto.AddMonths(gasto.QuantidadeParcelas);

            if (dataMax.Month >= DateTime.Today.Month || dataMax.Year >= DateTime.Today.Year)
                return true;

            return false;
        }

        private static bool ValidaGanho(Ganho ganho)
        {
            if (DateTime.Today.Month.Equals(ganho.DataDoGanho.Month))
                return true;
            else
                if (ganho.Recorrente && DateTime.Today.Month.Equals(ganho.DataDoGanho.Month))
                return true;

            return false;
        }

    }
}
