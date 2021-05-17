using NFinance.Domain;

namespace NFinance.Application.ViewModel.GanhoViewModel
{
    public class ConsultarGanhoViewModel
    {
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
            }
        }
    }
}