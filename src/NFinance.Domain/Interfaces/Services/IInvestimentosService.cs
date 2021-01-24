﻿using NFinance.Model.InvestimentosViewModel;
using System;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IInvestimentosService
    {
        Task<RealizarInvestimentoViewModel.Response> RealizarInvestimento(RealizarInvestimentoViewModel.Request request);
        Task<ListarInvestimentosViewModel.Response> ListarInvestimentos();
        Task<ConsultarInvestimentoViewModel.Response> ConsultarInvestimento(Guid id);
        Task<AtualizarInvestimentoViewModel.Response> AtualizarInvestimento(Guid id, AtualizarInvestimentoViewModel.Request request);
    }
}
