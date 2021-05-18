using System;
using System.Threading.Tasks;
using NFinance.Application.ViewModel.TelaInicialViewModel;

namespace NFinance.Application.Interfaces
{
    public interface ITelaInicialApp
    {
        public Task<TelaInicialViewModel> TelaInicial(Guid idCliente);

        public Task<GanhoMensalViewModel> GanhoMensal(Guid idCliente);

        public Task<GastoMensalViewModel> GastoMensal(Guid idCliente);

        public Task<InvestimentoMensalViewModel> InvestimentoMensal(Guid idCliente);

        public Task<ResgateMensalViewModel> ResgateMensal(Guid idCliente);
    }
}
