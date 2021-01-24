using NFinance.Domain;
using System.Collections.Generic;

namespace NFinance.Model.GastosViewModel
{
    public class ListarGastosViewModel
    {
        public class Response : List<GastoViewModel> 
        {
            public Response(List<Gastos> gastos)
            {
                foreach (var item in gastos)
                {
                    var gastoVm = new GastoViewModel(item);
                    this.Add(gastoVm);
                }
            }
        };

    }
}