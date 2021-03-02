using System;

namespace NFinance.Domain.ViewModel.GanhoViewModel
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
        }

        public class Response : GanhoViewModel
        {
            public Response() { }

            public Response(Ganho ganho)
            {
                Id = ganho.Id;
                IdCliente = ganho.IdCliente;
                NomeGanho = ganho.NomeGanho;
                Valor = ganho.Valor;
                Recorrente = ganho.Recorrente;
                DataDoGanho = ganho.DataDoGanho;
            }
        }
    }
}