using NFinance.Domain;
using System.Collections.Generic;

namespace NFinance.Application.ViewModel.ResgatesViewModel
{
    public class ConsultarResgatesViewModel
    {
        public class Response : List<ResgateViewModel.Response>
        {
            public Response(List<Resgate> resgate)
            {
                foreach (var item in resgate)
                {
                    var resgateVm = new ResgateViewModel.Response(item);
                    this.Add(resgateVm);
                }
            }
        };
    }
}