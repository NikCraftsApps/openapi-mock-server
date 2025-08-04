namespace OpenApiMockServer.Models;

public class Endpoint
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Route { get; set; } = "/";
    public string Method { get; set; } = "GET";
    public string Response { get; set; } = "{}";
    public int StatusCode { get; set; } = 200;
    public int CallCount { get; set; } = 0;
    public int DelayMs { get; set; } = 0;
}
