using System;
using NFinance.Domain;

namespace NFinance.Application.ViewModel.GanhoViewModel
{
    public class CadastrarGanhoViewModel
    {
        public class Request
        {
            public Guid IdCliente { get; set; }

            public string NomeGanho { get; set; }

            public decimal Valor { get; set; }

            public bool Recorrente { get; set; }

            public DateTime DataDoGanho { get; set; }

            public Request(){ }

            public Request(Ganho ganho)
            {
                IdCliente = ganho.IdCliente;
                NomeGanho = ganho.NomeGanho;
                Valor = ganho.Valor;
                Recorrente = ganho.Recorrente;
                DataDoGanho = ganho.DataDoGanho;
            }
        }

        public class Response : GanhoViewModel
        {
            public Response(Ganho ganho) : base(ganho)
            {
            }
        }
    }
}