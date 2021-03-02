using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NFinance.Domain.Services
{
    [ExcludeFromCodeCoverage]
    public static class TokenService
    {
        private static string _secret = "d53b997993b723080a619e8e5f575abf90df5c3c3fb332bd3cf3129f5057202f";

        public static string GerarToken(Cliente cliente)
        {
            if (cliente == null) throw new Exception("Usuario ou Senha Invalidos!");
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, cliente.Email),
                    new Claim(ClaimTypes.Name, cliente.Nome)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
