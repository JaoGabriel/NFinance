using NFinance.Domain;
using System.Collections.Generic;

namespace NFinance.WebApi.ViewModel.GastosViewModel
{
    public class ConsultarGastosViewModel
    {
        public class Response : List<GastoViewModel.Response>
        {
            public Response(List<Gasto> gastos)
            {
                foreach (var item in gastos)
                {
                    var gastoVm = new GastoViewModel.Response(item);
                    this.Add(gastoVm);
                }
            }
        };
    }
}