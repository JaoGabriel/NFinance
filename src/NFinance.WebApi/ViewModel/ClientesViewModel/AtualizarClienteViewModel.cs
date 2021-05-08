namespace NFinance.WebApi.ViewModel.ClientesViewModel
{
    public class AtualizarClienteViewModel
    {
        public class Request
        {
            public string Nome { get; set; }

            public string Cpf { get; set; }
            
            public string Email { get; set; }

            public string Senha { get; set; }

            public Request() { }

            public Request(ConsultarClienteViewModel.Response response)
            {
                Nome = response.Nome;
                Cpf = response.Cpf;
                Email = response.Email;
            }

        }

        public class Response : ClienteViewModel.Response { };
    }
}
