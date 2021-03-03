using NFinance.Model.InvestimentosViewModel;
using System.Collections.Generic;
using System.Linq;

namespace NFinance.Domain.ViewModel.TelaInicialViewModel
{
    public class InvestimentoMensalViewModel
    {
        public List<InvestimentoViewModel.Response> Investimentos { get; set; }

        public decimal SaldoMensal { get; set; }

        public InvestimentoMensalViewModel() { }

        public InvestimentoMensalViewModel(List<InvestimentoViewModel.Response> listInvestimentos)
        {
            Investimentos = listInvestimentos;
            SaldoMensal = listInvestimentos.Sum(s => s.Valor);
        }
    }
}
