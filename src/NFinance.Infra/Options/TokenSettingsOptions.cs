using System.Diagnostics.CodeAnalysis;

namespace NFinance.Infra.Options
{
    [ExcludeFromCodeCoverage]
    public class TokenSettingsOptions
    {
        public string TokenSecret { get; set; }
    }
}