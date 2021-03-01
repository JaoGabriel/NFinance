using NFinance.Domain;
using System.Collections.Generic;

namespace NFinance.Model.InvestimentosViewModel
{
    public class ConsultarInvestimentosViewModel
    {
        public class Response : ListarInvestimentosViewModel.Response 
        {
            public Response(List<Investimentos> investimentos) : base(investimentos)
            {
            }
        };
    }
}