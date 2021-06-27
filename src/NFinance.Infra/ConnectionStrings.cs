using NFinance.Infra.Options;
using Microsoft.Extensions.Options;

namespace NFinance.Infra
{
    public class ConnectionStrings
    {
        private readonly ConnectionStringsOptions _options;

        public ConnectionStrings(IOptions<ConnectionStringsOptions> options)
        {
            _options = options.Value;
        }
    }
}