using System;
using NFinance.Domain;

namespace NFinance.Application.ViewModel.GanhoViewModel
{
    public class AtualizarGanhoViewModel
    {
        public class Request
        {
            public Guid IdCliente { get; set; }

            public string NomeGanho { get; set; }

            public decimal Valor { get; set; }

            public bool Recorrente { get; set; }

            public DateTime DataDoGanho { get; set; }
        }

        public class Response : GanhoViewModel
        {
            public Response(Ganho ganho) : base(ganho)
            {
            }
        }
    }
}