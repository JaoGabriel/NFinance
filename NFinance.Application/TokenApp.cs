using System;
using System.Text;
using NFinance.Domain;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using NFinance.Infra.Identidade;

namespace NFinance.Application
{
    [ExcludeFromCodeCoverage]
    public static class TokenApp
    {
        private readonly static string _key = "d53b997993b723080a619e8e5f575abf90df5c3c3fb332bd3cf3129f5057202f";
        public static string GerarToken(Usuario usuario)
        {
            if (usuario == null) throw new ArgumentException("Ocorreu um erro ao efetuar o login!");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Hash, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.UserName),
                    new Claim(ClaimTypes.Email, usuario.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static List<string> LerToken(string authorization)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = authorization[7..];
            var payloadToken = tokenHandler.ReadJwtToken(token);
            var tokenValues = payloadToken.Payload.Values;
            var listaItens = new List<string>();
            foreach (var valores in tokenValues)
                listaItens.Add(valores.ToString());
            listaItens.Add(token);
            return listaItens;
        }
    }
}
