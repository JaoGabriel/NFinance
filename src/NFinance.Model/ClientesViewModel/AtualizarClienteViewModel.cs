namespace NFinance.Model.ClientesViewModel
{
    public class AtualizarClienteViewModel
    {
        public class Request
        {
            public string Nome { get; set; }

            public decimal RendaMensal { get; set; }
        }

        public class Response : ClienteViewModel { };
    }
}
