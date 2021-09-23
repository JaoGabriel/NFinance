using NFinance.Domain;

namespace NFinance.Application.ViewModel.InvestimentosViewModel
{
    public class ConsultarInvestimentoViewModel
    {
        public class Response : InvestimentoViewModel
        {
            public Response(Investimento investimentos) : base(investimentos)
            {
            }
        }
    }
}