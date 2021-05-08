using NFinance.Domain;
using System.Collections.Generic;

namespace NFinance.WebApi.ViewModel.InvestimentosViewModel
{
    public class ConsultarInvestimentosViewModel
    {
        public class Response : List<InvestimentoViewModel.Response>
        {
            public Response(List<Investimento> investimentos)
            {
                foreach (var item in investimentos)
                {
                    var investimentoVm = new InvestimentoViewModel.Response(item);
                    this.Add(investimentoVm);
                }
            }
        };
    }
}