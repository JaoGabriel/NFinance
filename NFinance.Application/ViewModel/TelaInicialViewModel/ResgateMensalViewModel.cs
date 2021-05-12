using System.Linq;
using System.Collections.Generic;
using NFinance.Application.ViewModel.ResgatesViewModel;

namespace NFinance.Application.ViewModel.TelaInicialViewModel
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
