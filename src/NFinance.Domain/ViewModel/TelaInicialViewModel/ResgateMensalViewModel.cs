using NFinance.Model.ResgatesViewModel;
using System.Collections.Generic;
using System.Linq;

namespace NFinance.Domain.ViewModel.TelaInicialViewModel
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
