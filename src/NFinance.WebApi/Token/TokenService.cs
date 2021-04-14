using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NFinance.Domain;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NFinance.WebApi.Token
{
    [ExcludeFromCodeCoverage]
    public class TokenService
    {
        private readonly IConfiguration Configuration;
        public TokenService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string GerarToken(Cliente cliente)
        {
            if (cliente == null) throw new ArgumentException("Usuario ou Senha Invalidos!");
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("TokenSettings").Value);
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
