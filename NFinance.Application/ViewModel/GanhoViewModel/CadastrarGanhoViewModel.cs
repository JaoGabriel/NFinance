using System;
using NFinance.Domain;

namespace NFinance.Application.ViewModel.GanhoViewModel
{
    public class CadastrarGanhoViewModel
    {
        public class Request : GanhoViewModel
        {
            public Request() { }

            public Request(Ganho ganho) : base(ganho) { }
        }

        public class Response : GanhoViewModel
        {
            public Response(Ganho ganho) : base(ganho) { }
        }
    }
}