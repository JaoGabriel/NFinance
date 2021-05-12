using System;
using System.Threading.Tasks;
using NFinance.Application.ViewModel.TelaInicialViewModel;

namespace NFinance.Application.Interfaces
{
    public interface ITelaInicialApp
    {
        public Task<TelaInicialViewModel> CarregarTelaInicial(Guid id);
    }
}
