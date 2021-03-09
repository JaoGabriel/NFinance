using System;

namespace NFinance.Domain.ViewModel.GanhoViewModel
{
    public class ExcluirGanhoViewModel
    {
        public class Request
        {
            public Guid IdGanho { get; set; }

            public Guid IdCliente { get; set; }

            public string MotivoExclusao { get; set; }
        }

        public class Response
        {
            public Response() { }

            public Response(bool status)
            {
                if (status)
                {
                    StatusCode = 200;
                    DataExclusao = DateTime.UtcNow;
                    Mensagem = "Excluido Com Sucesso";
                }
                else
                {
                    StatusCode = 400;
                    DataExclusao = DateTime.UtcNow;
                    Mensagem = "Ocorreu um erro ao Excluir";
                }
            }

            public int StatusCode { get; set; }

            public string Mensagem { get; set; }

            public DateTime DataExclusao { get; set; }
        }
    }
}