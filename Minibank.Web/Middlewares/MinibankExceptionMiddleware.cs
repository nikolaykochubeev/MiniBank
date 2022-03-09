using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Minibank.Core.Exceptions;

namespace Minibank.Web.Middlewares
{
    public class MinibankExceptionMiddleware
    {
        public readonly RequestDelegate next;

        public MinibankExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (MinibankException exception)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { Message = exception.Message });
            }
        }
    }
}