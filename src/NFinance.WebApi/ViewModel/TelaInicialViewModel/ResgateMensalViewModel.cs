using NFinance.WebApi.ViewModel.ResgatesViewModel;
using System.Collections.Generic;
using System.Linq;

namespace NFinance.WebApi.ViewModel.TelaInicialViewModel
{
    public class ResgateMensalViewModel
    {
        public List<ResgateViewModel.Response> Resgates { get; set; }

        public decimal SaldoMensal { get; set; }

        public ResgateMensalViewModel() { }

        public ResgateMensalViewModel(List<ResgateViewModel.Response> listResgates)
        {
            Resgates = listResgates;
            SaldoMensal = listResgates.Sum(s => s.Valor);
        }
    }
}
