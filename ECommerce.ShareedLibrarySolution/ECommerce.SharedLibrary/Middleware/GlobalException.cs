using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.SharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.SharedLibrary.Middleware
{
    internal class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            string message = " An error occurred while processing your request.";
            int statusCode = 500;
            string title = "Error";
            try
            {
                await next(context);
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    message = "You have made too many requests in a short period. Please try again later.";
                    statusCode = (int)StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(context, statusCode, title, message);
                }
                else if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Unauthorized";
                    message = "You are not authorized to access this resource.";
                    statusCode = (int)StatusCodes.Status401Unauthorized;
                    await ModifyHeader(context, statusCode, title, message);
                }
                else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Forbidden";
                    message = "You do not have permission to access this resource.";
                    statusCode = (int)StatusCodes.Status403Forbidden;
                    await ModifyHeader(context, statusCode, title, message);
                }
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                if (ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Timeout";
                    message = "The request timed out. Please try again later.";
                    statusCode = (int)StatusCodes.Status408RequestTimeout;
                    await ModifyHeader(context, statusCode, title, message);
                }
                else if (ex is SecurityTokenExpiredException)
                {
                    title = "Token Expired";
                    message = "Your token has expired. Please log in again.";
                    statusCode = (int)StatusCodes.Status401Unauthorized;
                    await ModifyHeader(context, statusCode, title, message);
                }
                else
                {
                    await ModifyHeader(context, statusCode, title, message);
                }
            }
        }
            private static async Task ModifyHeader(HttpContext context, int statusCode, string title, string message)
            {
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                var result = System.Text.Json.JsonSerializer.Serialize(new
                {
                    StatusCode = statusCode,
                    Title = title,
                    Message = message
                });
                await context.Response.WriteAsync(result);
            }
        }
    }
