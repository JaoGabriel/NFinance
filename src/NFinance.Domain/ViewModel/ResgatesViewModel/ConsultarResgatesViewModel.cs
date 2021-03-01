using NFinance.Domain;
using System.Collections.Generic;

namespace NFinance.Model.ResgatesViewModel
{
    public class ConsultarResgatesViewModel
    {
        public class Response : ListarResgatesViewModel.Response 
        {
            public Response(List<Resgate> resgates) : base(resgates)
            {
            }
        };
    }
}