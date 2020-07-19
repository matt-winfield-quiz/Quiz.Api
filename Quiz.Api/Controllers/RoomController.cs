using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Quiz.Api.Models.Display;
using Quiz.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Quiz.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private IRoomRepository _roomRepository;
        private IScoreRepository _scoreRepository;
        private ILogger<RoomController> _logger;

        public RoomController(IRoomRepository roomRepository, IScoreRepository scoreRepository, ILogger<RoomController> logger)
        {
            _roomRepository = roomRepository;
            _scoreRepository = scoreRepository;
            _logger = logger;
        }

        [HttpGet("rooms")]
        public ActionResult<IEnumerable<RoomDisplayModel>> GetRooms()
        {
            _logger.LogInformation("GetRooms OK");
            var rooms = _roomRepository.GetRooms()
                .Select(room => room.ToDisplayModel())
                .ToList();

            return Ok(rooms); ;
        }

        [HttpGet]
        public ActionResult<RoomDisplayModel> GetRoom([FromQuery] int roomId)
        {
            var room = _roomRepository.GetRoom(roomId);

            if (room != null)
            {
                _logger.LogInformation("GetRoom OK");
                return Ok(room);
            }
            _logger.LogInformation("GetRoom {roomId} not found", roomId);
            return NotFound(room);
        }

        [HttpGet("scores")]
        public ActionResult<Dictionary<string, int>> GetRoomScores([FromQuery] int roomId)
        {
            var scores = _scoreRepository.GetUserScores(roomId);

            _logger.LogInformation("GetRoomScores OK");
            return Ok(scores);
        }
    }
}