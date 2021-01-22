using AutoMapper;
using NFinance.Domain;
using NFinance.Model.InvestimentosViewModel;

namespace NFinance.Model.Mapper
{
    public class InvestimentosProfile : Profile
    {
        public InvestimentosProfile()
        {
            CreateMap<AtualizarInvestimentoViewModel.Request, Investimentos>();
            CreateMap<RealizarInvestimentoViewModel.Request, Investimentos>();
            CreateMap<Investimentos, AtualizarInvestimentoViewModel.Response>();
            CreateMap<Investimentos, RealizarInvestimentoViewModel.Response>();
            CreateMap<Investimentos, ConsultarInvestimentoViewModel.Response>();
            CreateMap<Investimentos, ListarInvestimentosViewModel.Response>();
            CreateMap<Investimentos, InvestimentoViewModel>();
        }
    }
}