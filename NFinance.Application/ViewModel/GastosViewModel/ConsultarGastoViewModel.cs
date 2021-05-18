using NFinance.Domain;

namespace NFinance.Application.ViewModel.GastosViewModel
{
    public class ConsultarGastoViewModel
    {
        public class Response : GastoViewModel.Response
        {
            public Response(Gasto gastos) : base(gastos)
            {
            }
        }
    }
}