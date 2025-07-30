namespace OpenApiMockServer.Models
{
    public class Endpoint
    {
        public string Route { get; set; } = "/";
        public string Method { get; set; } = "GET";
        public object Response { get; set; } = new { };
        public int? StatusCode { get; set; } = 200;
    }
}
