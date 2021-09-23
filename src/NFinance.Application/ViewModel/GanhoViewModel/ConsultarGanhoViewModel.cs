using NFinance.Domain;

namespace NFinance.Application.ViewModel.GanhoViewModel
{
    public class ConsultarGanhoViewModel
    {
        public class Response : GanhoViewModel
        {
            public Response(Ganho ganho) : base(ganho)
            {
            }
        }
    }
}