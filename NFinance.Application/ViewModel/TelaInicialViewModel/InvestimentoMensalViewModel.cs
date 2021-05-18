using System.Linq;
using System.Collections.Generic;
using NFinance.Application.ViewModel.InvestimentosViewModel;

namespace NFinance.Application.ViewModel.TelaInicialViewModel
{
    public class InvestimentoMensalViewModel
    {
        public List<InvestimentoViewModel> Investimentos { get; set; }

        public decimal SaldoMensal { get; set; }

        public InvestimentoMensalViewModel() { }

        public InvestimentoMensalViewModel(List<InvestimentoViewModel> listInvestimentos)
        {
            Investimentos = listInvestimentos;
            SaldoMensal = listInvestimentos.Sum(s => s.Valor);
        }
    }
}
