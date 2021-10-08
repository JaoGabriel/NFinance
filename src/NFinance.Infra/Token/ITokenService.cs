using NFinance.Domain.Identidade;

namespace NFinance.Infra.Token
{
    public interface ITokenService
    {
        string GerarToken(Usuario usuario);
    }
}