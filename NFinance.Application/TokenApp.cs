using System;
using System.Text;
using NFinance.Infra;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using NFinance.Domain.Identidade;

namespace NFinance.Application
{
    [ExcludeFromCodeCoverage]
    public static class TokenApp
    {
        private static readonly TokenSettings _tokenSettings;
        
        public static string GerarToken(Usuario usuario)
        {
            if (usuario == null) throw new ArgumentException("Ocorreu um erro ao efetuar o login!");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenSettings.ToString());
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim("IdUsuario",usuario.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
