using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Quiz.Api.Hubs
{
    public class BuzzerHub : Hub
    {
        private ILogger<BuzzerHub> _logger;

        public BuzzerHub(ILogger<BuzzerHub> logger)
        {
            _logger = logger;
        }
    }
}
