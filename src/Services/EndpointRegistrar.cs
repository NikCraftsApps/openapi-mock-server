using OpenApiMockServer.Models;
using Endpoint = OpenApiMockServer.Models.Endpoint;

namespace OpenApiMockServer.Services
{
    public class EndpointRegistrar
    {
        private readonly WebApplication _app;
        private readonly Dictionary<string, int> _stats = new();

        public EndpointRegistrar(WebApplication app)
        {
            _app = app;
        }

        public void Register(List<Endpoint> endpoints)
        {
            foreach (var endpoint in endpoints)
            {
                if (endpoint.Route == "/")
                    continue;

                switch (endpoint.Method.ToUpper())
                {
                    case "GET":
                        _app.MapGet(endpoint.Route, () =>
                        {
                            Increment(endpoint.Route);
                            return Results.Json(endpoint.Response, statusCode: endpoint.StatusCode ?? 200);
                        });
                        break;
                    case "POST":
                        _app.MapPost(endpoint.Route, () =>
                        {
                            Increment(endpoint.Route);
                            return Results.Json(endpoint.Response, statusCode: endpoint.StatusCode ?? 200);
                        });
                        break;
                    case "PUT":
                        _app.MapPut(endpoint.Route, () =>
                        {
                            Increment(endpoint.Route);
                            return Results.Json(endpoint.Response, statusCode: endpoint.StatusCode ?? 200);
                        });
                        break;
                    case "DELETE":
                        _app.MapDelete(endpoint.Route, () =>
                        {
                            Increment(endpoint.Route);
                            return Results.Json(endpoint.Response, statusCode: endpoint.StatusCode ?? 200);
                        });
                        break;
                }
            }

            // Endpoint /stats
            _app.MapGet("/stats", () => Results.Json(_stats));
        }

        private void Increment(string route)
        {
            if (!_stats.ContainsKey(route))
                _stats[route] = 0;
            _stats[route]++;
        }
    }
}
