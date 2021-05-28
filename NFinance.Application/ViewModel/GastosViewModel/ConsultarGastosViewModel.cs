﻿using NFinance.Domain;
using System.Collections.Generic;

namespace NFinance.Application.ViewModel.GastosViewModel
{
    public class ConsultarGastosViewModel
    {
        public class Response : List<GastoViewModel>
        {
            public Response(List<Gasto> gastos)
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