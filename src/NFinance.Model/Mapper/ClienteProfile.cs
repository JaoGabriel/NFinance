using AutoMapper;
using NFinance.Domain;
using NFinance.Model.ClientesViewModel;

namespace NFinance.Model.Mapper
{
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<AtualizarClienteViewModel.Request, Cliente>();
            CreateMap<CadastrarClienteViewModel.Request, Investimentos>();
            CreateMap<Cliente, AtualizarClienteViewModel.Response>();
            CreateMap<Cliente, CadastrarClienteViewModel.Response>();
            CreateMap<Cliente, ConsultarClienteViewModel.Response>();
            CreateMap<Cliente, ListarClientesViewModel.Response>();
            CreateMap<Cliente, ClienteViewModel>();
        }
    }
}
