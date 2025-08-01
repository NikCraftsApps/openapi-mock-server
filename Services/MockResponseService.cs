using OpenApiMockServer.Models;
using System.Text.Json;
using Endpoint = OpenApiMockServer.Models.Endpoint;

namespace OpenApiMockServer.Services
{
    public class MockResponseService
    {
        private readonly string _filePath;
        private readonly Dictionary<string, Endpoint> _endpoints = new();
        private readonly List<string> _logs = new();
        private const int MaxLogs = 200;

        public MockResponseService(string filePath)
        {
            _filePath = filePath;
            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "[]");
            Load();
        }

        public List<Endpoint> GetAll() => _endpoints.Values.ToList();

        public void Register(Endpoint endpoint)
        {
            var key = Key(endpoint.Method, endpoint.Route);
            _endpoints[key] = endpoint;
        }

        public bool TryGetEndpoint(string route, string method, out Endpoint? endpoint)
        {
            return _endpoints.TryGetValue(Key(method, route), out endpoint);
        }

        public void ClearStats()
        {
            foreach (var ep in _endpoints.Values)
                ep.CallCount = 0;
        }

        public void AddLog(string log)
        {
            _logs.Add(log);
            if (_logs.Count > MaxLogs)
                _logs.RemoveAt(0);
        }

        public IEnumerable<string> GetLogs() => _logs.ToList();

        public void Save(List<Endpoint> endpoints)
        {
            File.WriteAllText(_filePath, JsonSerializer.Serialize(endpoints, new JsonSerializerOptions { WriteIndented = true }));
            Load();
        }

        public List<Endpoint> Load()
        {
            var json = File.ReadAllText(_filePath);
            var endpoints = JsonSerializer.Deserialize<List<Endpoint>>(json) ?? new();
            _endpoints.Clear();
            foreach (var ep in endpoints)
                Register(ep);
            return endpoints;
        }

        private static string Key(string method, string route) => $"{method.ToUpper()}:{route}";
    }
}
