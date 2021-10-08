using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NFinance.Domain.Identidade;
using NFinance.Infra.Options;

namespace NFinance.Infra.Token
{
    public class TokenService : ITokenService
    {
        private readonly TokenSettingsOptions _tokenSettings;

        public TokenService(IOptions<TokenSettingsOptions> options)
        {
            _tokenSettings = options.Value;
        }
        
        public string GerarToken(Usuario usuario)
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
