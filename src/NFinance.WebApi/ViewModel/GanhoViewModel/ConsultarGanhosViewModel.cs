using System.Collections.Generic;

namespace NFinance.WebApi.ViewModel.GanhoViewModel
{
    public class ConsultarGanhosViewModel
    {
        public class Response : List<GanhoViewModel>
        {
            public Response(List<Ganho> ganhos)
            {
                foreach (var ganho in ganhos)
                {
                    var ganhoVm = new GanhoViewModel(ganho);
                    Add(ganhoVm);
                }
            }
        }
    }
}