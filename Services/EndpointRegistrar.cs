using OpenApiMockServer.Models;

namespace OpenApiMockServer.Services
{
    public class EndpointRegistrar
    {
        private readonly MockResponseService _service;

        public EndpointRegistrar(MockResponseService service)
        {
            _service = service;
        }

        public void RegisterEndpoints(WebApplication app)
        {
            foreach (var endpoint in _service.GetAll())
            {
                app.MapMethods(endpoint.Route, new[] { endpoint.Method }, async context =>
                {
                    if (_service.TryGetEndpoint(context.Request.Path, context.Request.Method, out var ep))
                    {
                        ep!.CallCount++;
                        context.Response.StatusCode = ep.StatusCode;
                        await context.Response.WriteAsync(ep.Response);
                    }
                    else
                    {
                        context.Response.StatusCode = 404;
                    }
                });
            }
        }
    }
}