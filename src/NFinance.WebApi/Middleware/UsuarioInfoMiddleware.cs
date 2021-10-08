using System;
using System.Linq;
using System.Text;
using NFinance.Infra;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using NFinance.Domain.ObjetosDeValor;

namespace NFinance.WebApi.Middleware
{
    public class UsuarioInfoMiddleware
    {
        private readonly RequestDelegate _next;
        private IConfiguration Configuration { get; }

        public UsuarioInfoMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            Configuration = configuration;
        }

        public async Task Invoke(HttpContext context, BaseDadosContext dataContext)
        {
            if (context.Request.Path.Value.Contains("/Autenticacao/Login") || context.Request.Path.Value.Contains("/Cliente/Cliente/Cadastrar"))
                await _next(context);
            
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
                var key = Encoding.ASCII.GetBytes(Configuration.GetSection("TokenSettings:TokenSecret").Value);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                
                var accountId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "IdUsuario").Value);
                var user = await dataContext.Users.FindAsync(accountId);
                var userInfo = new UsuarioInfo {IdUsuario = accountId, Email = user.Email};
                context.Items["Usuario"] = userInfo;
            }
            catch
            {
                throw new Exception("Ocorreu um erro tente novamente em instantes!");
            }
        }
    }
}