using EGPS.Application.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Helpers
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);
            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                var response = new ErrorResponse<object>
                {
                    success = false,
                    message = "You are unauthorized to perform this request",
                    errors = new { }
                };
                var responseJson = JsonConvert.SerializeObject(response, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(responseJson);
            }
        }
    }
}
