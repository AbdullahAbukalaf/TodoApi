using System.Net;
using TodoApi.Application.Abstractions;

namespace TodoApi.Middleware
{
    public class IdempotencyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TimeSpan _ttl = TimeSpan.FromMinutes(30);


        public IdempotencyMiddleware(RequestDelegate next) => _next = next;


        public async Task InvokeAsync(HttpContext context, IIdempotencyService idempo)
        {
            var method = context.Request.Method?.ToUpperInvariant();
            if (method == HttpMethods.Post || method == HttpMethods.Put || method == HttpMethods.Patch)
            {
                var key = context.Request.Headers["Idempotency-Key"].FirstOrDefault();
                var route = $"{context.Request.Method}:{context.Request.Path}";
                if (string.IsNullOrWhiteSpace(key))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsync("Idempotency-Key header is required.");
                    return;
                }


                if (await idempo.ExistsAsync(key!, route))
                {
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    await context.Response.WriteAsync("Duplicate request detected. Try a different Idempotency-Key.");
                    return;
                }


                await idempo.SetAsync(key!, route, _ttl);
            }


            await _next(context);
        }
    }
}
