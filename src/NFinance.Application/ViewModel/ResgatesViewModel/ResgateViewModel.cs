using System;
using NFinance.Domain;

namespace NFinance.Application.ViewModel.ResgatesViewModel
{
    public class ResgateViewModel
    {
        public Guid Id { get; set; }

        public Guid IdCliente { get; set; }

        public Guid IdInvestimento { get; set; }
        
        public decimal Valor { get; set; }
        
        public string MotivoResgate { get; set; }
        
        public DateTimeOffset DataResgate { get; set; }

        public ResgateViewModel() { }

        public ResgateViewModel(Resgate resgate)
        {
            Id = resgate.Id;
            Valor = resgate.Valor;
            MotivoResgate = resgate.MotivoResgate;
            DataResgate = resgate.DataResgate.LocalDateTime;
            IdInvestimento = resgate.IdInvestimento;
            IdCliente = resgate.IdCliente;
        }
    }
}