﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Quiz.Api.Models.Display;
using Quiz.Api.Repositories.Interfaces;
using System.Collections.Generic;

namespace Quiz.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private IRoomRepository _roomRepository;
        private ILogger<RoomController> _logger;

        public RoomController(IRoomRepository roomRepository, ILogger<RoomController> logger)
        {
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