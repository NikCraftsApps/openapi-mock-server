using OpenApiMockServer.Models;
using OpenApiMockServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
var logger = app.Logger;

app.Use(async (context, next) =>
{
    var method = context.Request.Method;
    var path = context.Request.Path;
    var timestamp = DateTime.UtcNow.ToString("o");
    logger.LogInformation("Request {Method} {Path} at {Timestamp}", method, path, timestamp);

    await next.Invoke();
});

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => new { status = "Mock Server is running" });

var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "endpoints.json");
var mockService = new MockResponseService(configPath);
var endpoints = mockService.LoadEndpoints();

var registrar = new EndpointRegistrar(app);
registrar.Register(endpoints);

app.Run();
