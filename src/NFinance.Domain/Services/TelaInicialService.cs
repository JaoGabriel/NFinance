using System;
using Cronos;
using System.Threading.Tasks;
using System.Collections.Generic;
using NFinance.Domain.Exceptions;
using NFinance.Domain.Interfaces.Services;
using NFinance.Domain.Interfaces.Repository;

namespace NFinance.Domain.Services
{
    public class TelaInicialService : ITelaInicialService
    {
        private readonly IGanhoRepository _ganhoRepository;
        private readonly IInvestimentoRepository _investimentoRepository;
        private readonly IGastoRepository _gastoRepository;
        private readonly IResgateRepository _resgateRepository;
        private readonly IClienteService _clienteService;

        public TelaInicialService(IGanhoRepository ganhoRepository, IInvestimentoRepository investimentoRepository, IGastoRepository gastoRepository, 
            IResgateRepository resgateRepository, IClienteService clienteService)
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
            var gastos = await _gastoRepository.ConsultarGastos(idCliente);
            var listGastos = new List<GastoViewModel.Response>();
            
            foreach (var gasto in gastos)
                if (ValidaGasto(gasto))
                {
                    var vm = new GastoViewModel.Response(gasto);
                    if (gasto.QuantidadeParcelas > 0)
                    {
                        if (ValidaProximoMes(DateTime.Today))
                        {
                            gasto.QuantidadeParcelas -= 1;
                            // sera que vale a pena salvar o valor parcela?
                            await _gastoRepository.AtualizarGasto(gasto.Id, gasto);
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

        private bool ValidaProximoMes(DateTime date)
        {
            var cron = CronExpression.Parse("* * 1 * *");
            var proxOcorrencia = cron.GetNextOccurrence(DateTime.UtcNow);

            if (proxOcorrencia?.Month == date.Month)
                return true;
            else
                return false;
        }

        private bool ValidaGasto(Gasto gasto)
        {
            if (gasto.DataDoGasto.Month > DateTime.Today.Month || gasto.DataDoGasto.Year > DateTime.Today.Year)
                return false;

            if (gasto.QuantidadeParcelas == 1)
                gasto.QuantidadeParcelas -= 1;

            var dataMax = gasto.DataDoGasto.AddMonths(gasto.QuantidadeParcelas);

            if (dataMax.Month >= DateTime.Today.Month)
                return true;
            
            return false;
        }

        private bool ValidaGanho(Ganho ganho)
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
