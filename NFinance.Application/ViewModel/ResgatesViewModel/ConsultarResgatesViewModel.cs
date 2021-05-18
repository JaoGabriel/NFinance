using NFinance.Domain;
using System.Collections.Generic;

namespace NFinance.Application.ViewModel.ResgatesViewModel
{
    public class ConsultarResgatesViewModel
    {
        public class Response : List<ResgateViewModel>
        {
            public Response(List<Resgate> resgate)
            {
                foreach (var item in resgate)
                {
                    var resgateVm = new ResgateViewModel(item);
                    this.Add(resgateVm);
                }
            }
        };
    }
}