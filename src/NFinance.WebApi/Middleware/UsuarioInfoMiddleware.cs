using System;
using System.Linq;
using System.Text;
using NFinance.Infra;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace NFinance.WebApi.Middleware
{
    public class UsuarioInfoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public UsuarioInfoMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, BaseDadosContext dataContext)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await IncluiUsuarioNoContexto(context, dataContext, token);

            await _next(context);
        }

        private async Task IncluiUsuarioNoContexto(HttpContext context, BaseDadosContext dataContext, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.TokenSettings.ToString());
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                
                context.Items["Account"] = await dataContext.Users.FindAsync(accountId);
            }
            catch
            {
                throw new Exception("Ocorreu um erro tente novamente em instantes!");
            }
        }
    }
}