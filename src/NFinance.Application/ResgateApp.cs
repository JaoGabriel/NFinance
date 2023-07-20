using System;
using NFinance.Domain;
using System.Threading.Tasks;
using NFinance.Application.Interfaces;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Application.ViewModel.ResgatesViewModel;
using NFinance.Application.Exceptions;

namespace NFinance.Application
{
    public class ResgateApp : IResgateApp
    {
        private readonly IResgateRepository _resgateRepository;

        public ResgateApp(IResgateRepository resgateRepository)
        {
            _resgateRepository = resgateRepository;
        }

        public async Task<ConsultarResgateViewModel.Response> ConsultarResgate(Guid idResgate)
        {
            ValidaId(idResgate);
            var resgate = await _resgateRepository.ConsultarResgate(idResgate);
            var resposta = new ConsultarResgateViewModel.Response(resgate);
            return resposta;
        }

        public async Task<ConsultarResgatesViewModel.Response> ConsultarResgates(Guid idCliente)
        {
            ValidaId(idCliente);
            var resgates = await _resgateRepository.ConsultarResgates(idCliente);
            var resposta = new ConsultarResgatesViewModel.Response(resgates);
            return resposta;
        }

        public async Task<RealizarResgateViewModel.Response> RealizarResgate(RealizarResgateViewModel.Request request)
        {
            var realizarResgate = new Resgate(request.IdInvestimento,request.IdCliente,request.Valor,request.MotivoResgate,request.DataResgate);
            var resgateRealizado = await _resgateRepository.RealizarResgate(realizarResgate);
            var resposta = new RealizarResgateViewModel.Response(resgateRealizado);
            return resposta;
        }

        private static void ValidaId(Guid id)
        {
            if (Guid.Empty.Equals(id))
                throw new ResgateException();
        }
    }
}
