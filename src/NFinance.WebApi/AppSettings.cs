using NFinance.Infra;

namespace NFinance.WebApi
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        
        public TokenSettings TokenSettings { get; set; }
    }
}