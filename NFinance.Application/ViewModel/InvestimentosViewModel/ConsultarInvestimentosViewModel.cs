using NFinance.Domain;
using System.Collections.Generic;

namespace NFinance.Application.ViewModel.InvestimentosViewModel
{
    public class ConsultarInvestimentosViewModel
    {
        public class Response : List<InvestimentoViewModel>
        {
            public Response(List<Investimento> investimentos)
            {
                foreach (var item in investimentos)
                {
                    var investimentoVm = new InvestimentoViewModel(item);
                    this.Add(investimentoVm);
                }
            }
        };
    }
}