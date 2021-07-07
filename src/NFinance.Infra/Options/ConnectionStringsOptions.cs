using System.Diagnostics.CodeAnalysis;

namespace NFinance.Infra.Options
{
    [ExcludeFromCodeCoverage]
    public class ConnectionStringsOptions
    {
        public string BancoDeDados { get; set; }

        public string Redis { get; set; }
    }
}