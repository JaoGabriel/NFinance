using NFinance.Domain;
using System;

namespace NFinance.Model.ClientesViewModel
{
    public class ClienteViewModel
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public decimal RendaMensal { get; set; }

        public ClienteViewModel() { }

        public ClienteViewModel(Cliente cliente)
        {
            Id = cliente.Id;
            Nome = cliente.Nome;
            RendaMensal = cliente.RendaMensal;
        }
    }
}
