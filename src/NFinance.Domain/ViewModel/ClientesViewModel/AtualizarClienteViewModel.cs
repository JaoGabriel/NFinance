namespace NFinance.Domain.ViewModel.ClientesViewModel
{
    public class AtualizarClienteViewModel
    {
        public class Request
        {
            public string Nome { get; set; }

            public string Cpf { get; set; }
            
            public string Email { get; set; }

            public string Senha { get; set; }

            public string Token { get; set; }

            public Request(ConsultarClienteViewModel.Response response,string token)
            {
                Nome = response.Nome;
                Cpf = response.Cpf;
                Email = response.Cpf;
                Token = token;
            }

        }

        public class Response : ClienteViewModel.Response { };
    }
}
