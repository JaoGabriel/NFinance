using System;

namespace NFinance.Application.ViewModel.GanhoViewModel
{
    public class ExcluirGanhoViewModel
    {
        public class Request
        {
            public Guid IdGanho { get; set; }

            public string MotivoExclusao { get; set; }
        }
        
       public class Response
        {
            public int StatusCode { get; set; }

            public string Mensagem { get; set; }

            public DateTimeOffset DataExclusao { get; set; }
            
            public Response() { }

            public Response(bool status)
            {
                if (status)
                {
                    StatusCode = 200;
                    DataExclusao = DateTimeOffset.Now;
                    Mensagem = "Excluido Com Sucesso";
                }
                else
                {
                    StatusCode = 400;
                    DataExclusao = DateTimeOffset.Now;
                    Mensagem = "Ocorreu um erro ao Excluir";
                }
            }
        }
    }
}