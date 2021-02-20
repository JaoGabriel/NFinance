using System.Collections.Generic;

namespace NFinance.Domain.ViewModel.GanhoViewModel
{
    public class ConsultarGanhosViewModel
    {
        public class Response : ListarGanhosViewModel.Response
        {
            public Response(List<Ganho> ganhos) : base(ganhos)
            {
            }
        }
    }
}