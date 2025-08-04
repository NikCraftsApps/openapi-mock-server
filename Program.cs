using OpenApiMockServer.Services;
using System.Text.Json;
using Endpoint = OpenApiMockServer.Models.Endpoint;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configPath = Path.Combine(Directory.GetCurrentDirectory(), "endpoints.json");
var mockService = new MockResponseService(configPath);
builder.Services.AddSingleton(mockService);

var app = builder.Build();

app.Use(async (context, next) =>
{
    var log = $"{DateTime.Now:HH:mm:ss} {context.Request.Method} {context.Request.Path}";
    mockService.AddLog(log);

    if (mockService.TryGetEndpoint(context.Request.Path, context.Request.Method, out var endpoint))
    {
        endpoint.CallCount++;

        if (endpoint.DelayMs > 0)
            await Task.Delay(endpoint.DelayMs);

        context.Response.StatusCode = endpoint.StatusCode;
        await context.Response.WriteAsync(endpoint.Response);
        return;
    }

    await next();
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mock API v1");
});

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/config", () => mockService.Load());

app.MapPost("/config/add", async (HttpContext ctx) =>
{
    var data = await ctx.Request.ReadFromJsonAsync<Dictionary<string, object>>();
    if (data == null) return Results.BadRequest();

    var response = "{}";
    if (data.ContainsKey("ResponseKey") && data.ContainsKey("ResponseValue"))
    {
        response = JsonSerializer.Serialize(new Dictionary<string, string>
        {
            { data["ResponseKey"].ToString()!, data["ResponseValue"].ToString()! }
        });
    }

    var statusCode = 200;
    if (data.ContainsKey("StatusCode"))
        int.TryParse(data["StatusCode"].ToString(), out statusCode);

    var delayMs = 0;
    if (data.ContainsKey("DelayMs"))
        int.TryParse(data["DelayMs"].ToString(), out delayMs);

    var endpoints = mockService.Load();
    var newEndpoint = new Endpoint
    {
        Id = Guid.NewGuid(),
        Route = data["Route"].ToString()!,
        Method = data["Method"].ToString()!,
        Response = response,
        StatusCode = statusCode,
        DelayMs = delayMs
    };
    endpoints.Add(newEndpoint);
    mockService.Save(endpoints);
    return Results.Ok();
});

app.MapPut("/config/update", async (HttpContext ctx) =>
{
    var ep = await ctx.Request.ReadFromJsonAsync<Endpoint>();
    if (ep == null || ep.Id == Guid.Empty) return Results.BadRequest();

    var endpoints = mockService.Load();
    var index = endpoints.FindIndex(e => e.Id == ep.Id);
    if (index == -1) return Results.NotFound();

    ep.CallCount = endpoints[index].CallCount;
    endpoints[index] = ep;
    mockService.Save(endpoints);
    return Results.Ok();
});

app.MapDelete("/config/delete", async (HttpContext ctx) =>
{
    var ep = await ctx.Request.ReadFromJsonAsync<Endpoint>();
    if (ep == null || ep.Id == Guid.Empty) return Results.BadRequest();

    var endpoints = mockService.Load();
    endpoints.RemoveAll(e => e.Id == ep.Id);
    mockService.Save(endpoints);
    return Results.Ok();
});
app.MapGet("/config/export", (HttpContext ctx) =>
{
    var endpoints = mockService.Load();
    return Results.Json(endpoints, new JsonSerializerOptions { WriteIndented = true });
});

app.MapPost("/config/import", async (HttpContext ctx) =>
{
    var endpoints = await ctx.Request.ReadFromJsonAsync<List<Endpoint>>();
    if (endpoints == null) return Results.BadRequest();
    mockService.Save(endpoints);
    return Results.Ok();
});


app.MapGet("/stats", () => mockService.GetAll().Select(e => new { e.Route, e.Method, e.CallCount }));
app.MapPost("/stats/clear", () => { mockService.ClearStats(); return Results.Ok(); });
app.MapGet("/logs", () => mockService.GetLogs());

app.MapGet("/", () => new { status = "Mock Server is running" });

app.Run("http://localhost:5000");
