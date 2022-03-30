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
            catch (ValidationException exception)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { Message = exception.Message });
            }
            catch (ObjectNotFoundException exception)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                await httpContext.Response.WriteAsJsonAsync(new { Message = exception.Message });
            }
        }
    }
}