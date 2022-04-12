using System;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.Enum;
using CHESF.COMPRAS.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace CHESF.COMPRAS.API.Middlewares
{
    public class JwtTokenMiddleware
    {
        
        private readonly RequestDelegate _next;

        public JwtTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                var tokenAtual =
                    await httpContext.GetTokenAsync("access_token");

                if (tokenAtual != null)
                {
                    var statusToken = TokenService.ValidarToken(tokenAtual);
                    var novoToken = "Bearer ";

                    switch (statusToken)
                    {
                        case StatusToken.Refresh:
                            var claims = TokenService.GetPrincipalFromToken(tokenAtual);
                            novoToken += TokenService.GenerateToken(claims);
                            break;
                        case StatusToken.Valido:
                            novoToken += tokenAtual;
                            break;
                        default:
                            novoToken = null;
                            break;
                    }

                    httpContext.Response.Headers.Add("Authorization",  novoToken);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            await _next(httpContext);
        }
    }
}