namespace OpenApiMockServer.Services
{
    public class LogService
    {
        private readonly List<string> _logs = new();
        private const int MaxLogs = 200;

        public void Add(string log)
        {
            _logs.Add(log);
            if (_logs.Count > MaxLogs)
                _logs.RemoveAt(0);
        }

        public IEnumerable<string> Get() => _logs.ToList();
    }
}
