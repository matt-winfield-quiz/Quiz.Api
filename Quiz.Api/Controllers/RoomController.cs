using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Quiz.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private ILogger<RoomController> _logger;

        public RoomController(ILogger<RoomController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<int> GetRoom([FromQuery] int id)
        {
            _logger.LogInformation("GetRoom called");
            return 0;
        }
    }
}