using AutoMapper;
using NFinance.Domain;
using NFinance.Model.ResgatesViewModel;

namespace NFinance.Model.Mapper
{
    public class ResgateProfile : Profile
    {
        public ResgateProfile()
        {
            CreateMap<RealizarResgateViewModel.Request, Resgate>();
            CreateMap<Investimentos, RealizarResgateViewModel.Response>();
            CreateMap<Investimentos, ConsultarResgateViewModel.Response>();
            CreateMap<Investimentos, ListarResgatesViewModel.Response>();
            CreateMap<Investimentos, ResgateViewModel>();
        }
    }
}