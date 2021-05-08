using NFinance.WebApi.ViewModel.GastosViewModel;
using System.Collections.Generic;
using System.Linq;

namespace NFinance.WebApi.ViewModel.TelaInicialViewModel
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
