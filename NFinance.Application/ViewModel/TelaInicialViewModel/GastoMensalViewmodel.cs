using System.Linq;
using System.Collections.Generic;
using NFinance.Application.ViewModel.GastosViewModel;

namespace NFinance.Application.ViewModel.TelaInicialViewModel
{
    public class GastoMensalViewModel
    {
        public List<GastoViewModel.Response> Gastos { get; set; }

        public decimal SaldoMensal { get; set; }

        public GastoMensalViewModel() { }

        public GastoMensalViewModel(List<GastoViewModel.Response> listGastos)
        {
            Gastos = listGastos;
            SaldoMensal = listGastos.Sum(s => s.Valor);
        }
    }
}
