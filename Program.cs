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

    var endpoints = mockService.Load();
    var statusCode = 200;
    if (data.ContainsKey("StatusCode"))
        int.TryParse(data["StatusCode"].ToString(), out statusCode);

    var newEndpoint = new Endpoint
    {
        Id = Guid.NewGuid(),
        Route = data["Route"].ToString()!,
        Method = data["Method"].ToString()!,
        Response = response,
        StatusCode = statusCode
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

app.MapGet("/stats", () => mockService.GetAll().Select(e => new { e.Route, e.Method, e.CallCount }));
app.MapPost("/stats/clear", () => { mockService.ClearStats(); return Results.Ok(); });

app.MapGet("/logs", () => mockService.GetLogs());

var registrar = new EndpointRegistrar(mockService);
registrar.RegisterEndpoints(app);

app.MapGet("/", () => new { status = "Mock Server is running" });

app.Run("http://localhost:5000");
