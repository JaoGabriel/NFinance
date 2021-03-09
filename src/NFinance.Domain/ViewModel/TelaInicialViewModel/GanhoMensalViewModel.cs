using System.Collections.Generic;
using System.Linq;

namespace NFinance.Domain.ViewModel.TelaInicialViewModel
{
    public class GanhoMensalViewModel
    {
        public List<GanhoViewModel.GanhoViewModel> Ganhos { get; set; }

        public decimal SaldoMensal { get; set; }

        public GanhoMensalViewModel() { }

        public GanhoMensalViewModel(List<GanhoViewModel.GanhoViewModel> listGanhos)
        {
            Ganhos = listGanhos;
            SaldoMensal = listGanhos.Sum(s => s.Valor);
        }
    }
}
