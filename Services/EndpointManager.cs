using System.Text.Json;
using OpenApiMockServer.Models;
using Endpoint = OpenApiMockServer.Models.Endpoint;

namespace OpenApiMockServer.Services;

public class EndpointManager
{
    private readonly string _filePath;

    public EndpointManager(string filePath)
    {
        _filePath = filePath;
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public List<Endpoint> Load()
    {
        var json = File.ReadAllText(_filePath);
        var endpoints = JsonSerializer.Deserialize<List<Endpoint>>(json) ?? new();

        foreach (var ep in endpoints.Where(e => e.Id == Guid.Empty))
            ep.Id = Guid.NewGuid();

        return endpoints;
    }

    public void Save(List<Endpoint> endpoints)
    {
        File.WriteAllText(_filePath, JsonSerializer.Serialize(endpoints, new JsonSerializerOptions { WriteIndented = true }));
    }
}
