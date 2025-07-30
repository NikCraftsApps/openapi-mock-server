using System.Text.Json;
using OpenApiMockServer.Models;
using Endpoint = OpenApiMockServer.Models.Endpoint;

namespace OpenApiMockServer.Services
{
    public class MockResponseService
    {
        private readonly string _filePath;
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public MockResponseService(string filePath)
        {
            _filePath = filePath;
        }

        public List<Endpoint> LoadEndpoints()
        {
            Console.WriteLine($"[MockResponseService] Loading endpoints from {_filePath}");

            if (!File.Exists(_filePath))
            {
                Console.WriteLine($"[MockResponseService] File not found: {_filePath}");
                return new List<Endpoint>();
            }

            try
            {
                var json = File.ReadAllText(_filePath);
                var endpoints = JsonSerializer.Deserialize<List<Endpoint>>(json, _options);

                Console.WriteLine($"[MockResponseService] Loaded {endpoints?.Count ?? 0} endpoints");
                return endpoints ?? new List<Endpoint>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MockResponseService] Error reading file: {ex.Message}");
                return new List<Endpoint>();
            }
        }
    }
}
