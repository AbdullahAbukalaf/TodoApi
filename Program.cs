using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Threading.RateLimiting;
using TodoApi.Application.Abstractions;
using TodoApi.Application.Dtos;
using TodoApi.Application.Mapping;
using TodoApi.Application.Services;
using TodoApi.Application.Validation;
using TodoApi.Infrastructure.Persistence;
using TodoApi.Infrastructure.Repositories;
using TodoApi.Middleware;
using TodoApi.Swagger;

var builder = WebApplication.CreateBuilder(args);
// Serilog
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("Default", p => p
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin());
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Version = "v1" });
    c.OperationFilter<AddIdempotencyHeaderOperationFilter>();
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(TodoProfile).Assembly);

// FluentValidation
builder.Services.AddScoped<IValidator<CreateTodoRequest>, CreateTodoRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateTodoRequest>, UpdateTodoRequestValidator>();

// Rate Limiting (fixed window)
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    RateLimitPartition.GetFixedWindowLimiter(
    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "global",
    factory: _ => new FixedWindowRateLimiterOptions
    {
        PermitLimit = 60, // 60 requests
        Window = TimeSpan.FromMinutes(1),
        QueueLimit = 0,
        AutoReplenishment = true
    }));
});

// Idempotency
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IIdempotencyService, IdempotencyService>();

// Repos & Services
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();

// Auth (optional) — wire but not required
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    // options.TokenValidationParameters = ...
});

var app = builder.Build();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("Default");
app.UseRateLimiter();
app.UseMiddleware<IdempotencyMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseDefaultFiles();   // serves index.html by default
app.UseStaticFiles();    // serves files from wwwroot/

app.MapControllers();

app.Run();
