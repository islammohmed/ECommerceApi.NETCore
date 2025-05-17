namespace ApiGateway.Presentation.Middleware
{
    public class AttachSignatureToRequest(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var signedHeader = context.Request.Headers["X-ApiGateway-Signature"];
            if (signedHeader.FirstOrDefault() is null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("503 Service Unavailable");
                return;
            }
            else
            {
                context.Request.Headers.Add("X-ApiGateway-Signature", signedHeader);
                await next(context);
            }
        }
    }
}
