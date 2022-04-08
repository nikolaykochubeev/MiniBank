using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Minibank.Web.Middlewares
{
    public class ExceptionMiddleware
    {
        public readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (FluentValidation.ValidationException exception)
            {
                var errors = exception.Errors
                    .Select(x => $"{x.PropertyName}: {x.ErrorMessage}");
                var errorMessage = string.Join(Environment.NewLine, errors);
                await httpContext.Response.WriteAsJsonAsync(new { Error = errorMessage });
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { Message = exception.Message });
            }
        }
    }
}