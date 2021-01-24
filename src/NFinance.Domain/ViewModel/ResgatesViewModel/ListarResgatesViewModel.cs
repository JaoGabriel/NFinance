using NFinance.Domain;
using System.Collections.Generic;

namespace NFinance.Model.ResgatesViewModel
{
    public class ListarResgatesViewModel
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