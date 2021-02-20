using System.Collections.Generic;

namespace NFinance.Domain.ViewModel.GanhoViewModel
{
    public class ListarGanhosViewModel
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