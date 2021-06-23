using System.Collections.Generic;

namespace NFinance.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        string GerarToken(Cliente cliente);
        
        List<string> LerToken(string authorization);
    }
}