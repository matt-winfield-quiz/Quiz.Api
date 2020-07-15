using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quiz.Api.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        [HttpGet("rooms")]
        public ActionResult<IEnumerable<Room>> GetRooms()
        {
            return new List<Room>()
            {
                new Room()
            };
        }
    }
}