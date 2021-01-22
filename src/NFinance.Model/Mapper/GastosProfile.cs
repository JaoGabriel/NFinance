using AutoMapper;
using NFinance.Domain;
using NFinance.Model.GastosViewModel;

namespace NFinance.Model.Mapper
{
    public class GastosProfile : Profile
    {
        public GastosProfile()
        {
            CreateMap<AtualizarGastoViewModel.Request, Gastos>();
            CreateMap<CadastrarGastoViewModel.Request, Gastos>();
            CreateMap<ExcluirGastoViewModel.Request, Gastos>();
            CreateMap<Gastos, AtualizarGastoViewModel.Response>();
            CreateMap<Gastos, CadastrarGastoViewModel.Response>();
            CreateMap<Gastos, ExcluirGastoViewModel.Response>();
            CreateMap<Gastos, ConsultarGastoViewModel.Response>();
            CreateMap<Gastos, ListarGastosViewModel.Response>();
            CreateMap<Gastos, GastoViewModel>();
        }
    }
}