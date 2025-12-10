using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TodoApi.Swagger
{
    public class AddIdempotencyHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.ApiDescription.HttpMethod?.ToUpperInvariant();
            if (method is not ("POST" or "PUT" or "PATCH")) return;

            operation.Parameters ??= new List<OpenApiParameter>();
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Idempotency-Key",
                In = ParameterLocation.Header,
                Required = true,
                Description = "Unique key to make this write operation idempotent (e.g., a GUID).",
                Schema = new OpenApiSchema { Type = "string" }
            });
        }
    }
}
