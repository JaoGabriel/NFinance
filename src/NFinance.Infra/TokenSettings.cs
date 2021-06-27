using NFinance.Infra.Options;
using Microsoft.Extensions.Options;

namespace NFinance.Infra
{
    public class TokenSettings
    {
        private readonly TokenSettingsOptions _options;

        public TokenSettings(IOptions<TokenSettingsOptions> options)
        {
            _options = options.Value;
        }
    }
}