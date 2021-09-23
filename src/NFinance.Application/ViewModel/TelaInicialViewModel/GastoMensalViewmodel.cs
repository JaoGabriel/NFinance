using System.Linq;
using System.Collections.Generic;
using NFinance.Application.ViewModel.GastosViewModel;

namespace NFinance.Application.ViewModel.TelaInicialViewModel
{
    public class GastoMensalViewModel
    {
        public List<GastoViewModel> Gastos { get; set; }

        public decimal SaldoMensal { get; set; }

        public GastoMensalViewModel() { }

        public GastoMensalViewModel(List<GastoViewModel> listGastos)
        {
            Gastos = listGastos;
            SaldoMensal = listGastos.Sum(s => s.Valor);
        }
    }
}
