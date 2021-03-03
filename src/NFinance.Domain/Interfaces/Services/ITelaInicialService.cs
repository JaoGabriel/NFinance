using NFinance.Domain.ViewModel.TelaInicialViewModel;
using System;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Services
{
    public interface ITelaInicialService
    {
        public Task<TelaInicialViewModel> TelaInicial(Guid idCliente);

        public Task<GanhoMensalViewModel> GanhoMensal(Guid idCliente);

        public Task<GastoMensalViewModel> GastoMensal(Guid idCliente);

        public Task<InvestimentoMensalViewModel> InvestimentoMensal(Guid idCliente);

        public Task<ResgateMensalViewModel> ResgateMensal(Guid idCliente);
    }
}
