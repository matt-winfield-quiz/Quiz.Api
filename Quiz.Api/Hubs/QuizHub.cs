using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Quiz.Api.Models.Internal;
using Quiz.Api.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace Quiz.Api.Hubs
{
    public class QuizHub : Hub
    {
        private IRoomRepository _roomRepository;
        private ILogger<QuizHub> _logger;

        public QuizHub(IRoomRepository roomRepository, ILogger<QuizHub> logger)
        {
            _roomRepository = roomRepository;
            _logger = logger;
        }

        public int CreateRoom(string roomName, string roomPassword)
        {
            var roomId = _roomRepository.AddRoom(new RoomInternalModel
            {
                Name = roomName,
                Password = roomPassword,
                OwnerUserIdentifier = Context.ConnectionId
            });
            _logger.LogInformation("Created room {roomId} {roomName}", roomId, roomName);
            return roomId;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
