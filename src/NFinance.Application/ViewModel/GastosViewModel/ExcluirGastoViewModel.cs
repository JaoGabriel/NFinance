using System;

namespace NFinance.Application.ViewModel.GastosViewModel
{
    public class ExcluirGastoViewModel
    {
        public class Request
        {
            public Guid IdGasto { get; set; }

            public Guid IdCliente { get; set; }

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
                    DataExclusao = DateTimeOffset.Now.DateTime;
                    Mensagem = "Excluido Com Sucesso";
                }
                else
                {
                    StatusCode = 400;
                    DataExclusao = DateTimeOffset.Now.DateTime;
                    Mensagem = "Ocorreu um erro ao Excluir";
                }
            }
        }
    }
}