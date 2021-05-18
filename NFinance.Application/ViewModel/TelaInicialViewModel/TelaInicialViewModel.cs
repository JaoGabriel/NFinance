using NFinance.Domain;

namespace NFinance.Application.ViewModel.TelaInicialViewModel
{
    public class TelaInicialViewModel
    {
        public Cliente Cliente { get; set; }

        public GanhoMensalViewModel GanhoMensal { get; set; }

        public GastoMensalViewModel GastoMensal { get; set; }

        public InvestimentoMensalViewModel InvestimentoMensal { get; set; }

        public ResgateMensalViewModel ResgateMensal { get; set; }

        public decimal ResumoMensal { get; set; }

        public TelaInicialViewModel(Cliente cliente, GanhoMensalViewModel ganhoMensal, GastoMensalViewModel gastoMensal, 
            InvestimentoMensalViewModel investimentoMensal, ResgateMensalViewModel resgateMensal, decimal resumoMensal)
        {
            Cliente = cliente;
            GanhoMensal = ganhoMensal;
            GastoMensal = gastoMensal;
            InvestimentoMensal = investimentoMensal;
            ResgateMensal = resgateMensal;
            ResumoMensal = resumoMensal;
        }
    }
}
