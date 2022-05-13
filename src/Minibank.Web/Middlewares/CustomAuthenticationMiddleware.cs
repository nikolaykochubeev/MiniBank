using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Minibank.Web.Middlewares
{
    public class CustomAuthenticationMiddleware
    {
        public readonly RequestDelegate next;

        public CustomAuthenticationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(token))
            {
                var payloadDictionary =
                    JsonSerializer.Deserialize<Dictionary<string, object>>(
                        Base64UrlEncoder.Decode(token.Split(" ")[1].Split(".")[1]));

                var exp = int.Parse(payloadDictionary!["exp"].ToString()!);

                if (exp < DateTimeOffset.Now.ToUnixTimeSeconds())
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    await httpContext.Response.WriteAsJsonAsync(new { Message = "Token expired" });
                    return;
                }
            }

            await next(httpContext);
        }
    }
}