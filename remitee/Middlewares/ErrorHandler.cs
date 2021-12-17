using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using remitee.DTOs;
using remitee.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace remitee.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ErrorHandler
    {
        private readonly RequestDelegate _next;

        public ErrorHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(Exception ex)
            {
                var response = httpContext.Response;
                response.ContentType = "application/json";
                ErrorResponse errorResponse = new ErrorResponse();

                switch (ex)
                {
                    case AppException e:
                        errorResponse.StatusCode = e.StatusCode;
                        errorResponse.Message = e.Message;
                        response.StatusCode = e.StatusCode;
                        break;
                    default:
                        errorResponse.StatusCode = 500;
                        errorResponse.Message = "Error general";
                        response.StatusCode = 500;
                        break;
                }

                var result = JsonConvert.SerializeObject(errorResponse);
                await response.WriteAsync(result);
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ErrorHandlerExtensions
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandler>();
        }
    }
}
