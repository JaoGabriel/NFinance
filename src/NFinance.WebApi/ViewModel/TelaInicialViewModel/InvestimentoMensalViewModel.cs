using NFinance.WebApi.ViewModel.InvestimentosViewModel;
using System.Collections.Generic;
using System.Linq;

namespace NFinance.WebApi.ViewModel.TelaInicialViewModel
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
