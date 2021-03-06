using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NFinance.WebApi
{
    public class UsuarioMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MemoryCache _cache;

        public UsuarioMiddleware(RequestDelegate next, MemoryCache cache)
        {
            _next = next;
            _cache = cache;
        }

        // IMyScopedService is injected into Invoke
        public async Task Invoke(HttpContext httpContext)
        {
            var responseHttp = httpContext.Response.Body.ToString();
            
            await _next(httpContext);
        }
    }
}
