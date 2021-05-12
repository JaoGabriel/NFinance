namespace NFinance.Application.ViewModel
{
    public class ErroViewModel
    {

        public int Codigo { get; set; }
        
        public string Mensagem { get; set; }
        
        public ErroViewModel(int codigo, string mensagem)
        {
            Codigo = codigo;
            Mensagem = mensagem;
        }
    }
}
