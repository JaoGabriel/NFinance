using Microsoft.EntityFrameworkCore;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NFinance.Infra.Repository
{
    public class PainelDeControleRepository : IPainelDeControleRepository
    {
        private readonly IInvestimentosRepository _investimentoRepository;
        private readonly IGastosRepository _gastosRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly BaseDadosContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public PainelDeControleRepository(BaseDadosContext context, IInvestimentosRepository investimentosRepository, IGastosRepository gastosRepository, IClienteRepository clienteRepository)
        {
            _context = context;
            _investimentoRepository = investimentosRepository;
            _gastosRepository = gastosRepository;
            _clienteRepository = clienteRepository;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public Task<PainelDeControle> PainelDeControle(Guid id)
        {
            throw new NotImplementedException();

            //var response = new PainelDeControle() { ValorInvestidoMensal = ,SaldoMensal = ,GastosMensal = ,ValorInvestidoAnual = , SaldoAnual = ,GastosAnual = ,ValorRecebidoAnual =  };
            //return response;
        }

        private Task SaldoMensal()
        {
            var mesAtual = DateTime.UtcNow.Month;
            throw new NotImplementedException();

            //Arrumar o investimento / gasto fazendo com que toda vez que for inserido um gasto ele gerar uma lista com todos os gastos do cliente
        }

        private async Task InvestimentoMensal()
        {
            var mesAtual = DateTime.UtcNow.Month;
            await _context.Investimentos.FirstOrDefaultAsync(i => i.DataAplicacao.Month == mesAtual);
        }

        private async Task<decimal> GastoMensal()
        {
            var mesAtual = DateTime.UtcNow.Month;
            var gastoMensal = await _context.Gastos.FirstOrDefaultAsync(g => g.DataDoGasto.Month == mesAtual);
            return gastoMensal.ValorTotal;
        }

        private async Task<decimal> InvestimentoAnual(Guid id)
        {
            var listaInvestimentos = await _investimentoRepository.ListarInvestimentos();
            var response = listaInvestimentos.Sum(i => i.Valor);
            return response;
        }

        private async Task<decimal> RecebidoAnual(Guid id)
        {
            var cliente = await _clienteRepository.ConsultarCliente(id);
            var recebidoAnual = cliente.RendaMensal * 12;
            return recebidoAnual;
        }

        private async Task<decimal> GastosAnual(Guid id)
        {
            var gastos = await _gastosRepository.ListarGastos();
            var gastosAnual = gastos.Sum(i => i.ValorTotal);
            return gastosAnual;
        }
    }
}
