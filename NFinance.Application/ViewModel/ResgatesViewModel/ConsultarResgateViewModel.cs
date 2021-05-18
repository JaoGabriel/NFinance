using NFinance.Domain;

namespace NFinance.Application.ViewModel.ResgatesViewModel
{
    public class ConsultarResgateViewModel
    {
        public class Response : ResgateViewModel
        {
            public Response(Resgate resgate) : base(resgate)
            {
            }
        }
    }
}