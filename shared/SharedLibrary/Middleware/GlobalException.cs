using SharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using Serilog;

namespace SharedLibrary.Middleware
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            //Declare default variable
            string message = "Sorry, internal server error occurred. Kindly try again";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "Error";
            try
            {
                await next(context);
                //check if Exception is Too many Request // 429 status code
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    message = "Too many request made";
                    statusCode = (int)StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(context, title, message, statusCode);
                }

                //if response is UnAuthorized //401 status code
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized) {
                    title = "Alert";
                    message = "You are not authorized to access.";
                    await ModifyHeader(context, title, message,statusCode);
                }

                //if response is forbidden //403 status code
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Out of Access";
                    message = "You are not allowed/required to access.";
                    statusCode = StatusCodes.Status403Forbidden;
                    await ModifyHeader(context, title, message, statusCode);
                }
            }
            catch (Exception ex)
            {
                //Log Original Exceptions /File, Debugger, Console 
                LogException.LogExceptions(ex);

                // Log chi tiết lỗi
                Log.Error($"Global exception caught: {ex.Message}\nStackTrace: {ex.StackTrace}");

                //check if exception is timeout //408 request timeout 
                if (ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Out of time";
                    message = "Request timeout ... try again";
                    statusCode = StatusCodes.Status408RequestTimeout;
                }
                //if none of the exceptions then do the default 
                //if exception is caught
                await ModifyHeader(context, title, message, statusCode);
            }
        }

        private async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            //display scary-free message to client 
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
            {
                Detail = message,
                Status = statusCode,
                Title = title
            }), CancellationToken.None); 
            return;
        }
    }
}