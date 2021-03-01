using NFinance.Domain;
using System.Collections.Generic;

namespace NFinance.Model.GastosViewModel
{
    public class ConsultarGastosViewModel
    {
        public class Response : ListarGastosViewModel.Response 
        {
            public Response(List<Gastos> gastos) : base(gastos)
            {
            }
        };
    }
}