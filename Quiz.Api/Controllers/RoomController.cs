using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Quiz.Api.Hubs;
using Quiz.Api.Models.Display;
using Quiz.Api.Models.Internal;
using Quiz.Api.Repositories.Interfaces;
using System.Collections.Generic;

namespace Quiz.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private IHubContext<QuizHub> _quizHubContext;
        private IRoomRepository _roomRepository;
        private ILogger<RoomController> _logger;

        public RoomController(IHubContext<QuizHub> quizHubContext, IRoomRepository roomRepository, ILogger<RoomController> logger)
        {
            _quizHubContext = quizHubContext;
            _roomRepository = roomRepository;
            _logger = logger;
        }

        [HttpGet("rooms")]
        public ActionResult<IEnumerable<RoomDisplayModel>> GetRooms()
        {
            _logger.LogInformation("GetRooms 200 OK");
            return Ok(_roomRepository.GetRooms());
        }
    }
}