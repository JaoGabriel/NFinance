using NFinance.Domain.ViewModel.ClientesViewModel;
using System;

namespace NFinance.Domain.ViewModel.TelaInicialViewModel
{
    public class TelaInicialViewModel
    {
        public Guid Id { get; set; }

        public ConsultarClienteViewModel.Response Cliente { get; set; }

        public GanhoMensalViewModel GanhoMensal { get; set; }

        public GastoMensalViewModel GastoMensal { get; set; }

        public InvestimentoMensalViewModel InvestimentoMensal { get; set; }

        public ResgateMensalViewModel ResgateMensal { get; set; }

        public decimal ResumoMensal { get; set; }

        public TelaInicialViewModel(ConsultarClienteViewModel.Response cliente, GanhoMensalViewModel ganhoMensal, GastoMensalViewModel gastoMensal, 
            InvestimentoMensalViewModel investimentoMensal, ResgateMensalViewModel resgateMensal, decimal resumoMensal)
        {
            Id = Guid.NewGuid();
            Cliente = cliente;
            GanhoMensal = ganhoMensal;
            GastoMensal = gastoMensal;
            InvestimentoMensal = investimentoMensal;
            ResgateMensal = resgateMensal;
            ResumoMensal = resumoMensal;
        }
    }
}
