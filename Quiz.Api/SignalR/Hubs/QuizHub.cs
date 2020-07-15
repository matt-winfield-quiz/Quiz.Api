using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Quiz.Api.Jwt;
using Quiz.Api.Models.Internal;
using Quiz.Api.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace Quiz.Api.SignalR.Hubs
{
    public class QuizHub : Hub
    {
        private IRoomRepository _roomRepository;
        private JwtManager _jwtManager;
        private ILogger<QuizHub> _logger;

        public QuizHub(IRoomRepository roomRepository, JwtManager jwtManager, ILogger<QuizHub> logger)
        {
            _roomRepository = roomRepository;
            _jwtManager = jwtManager;
            _logger = logger;
        }

        public Task CreateRoom(string roomName, string roomPassword)
        {
            var roomId = _roomRepository.AddRoom(new RoomInternalModel
            {
                Name = roomName,
                Password = roomPassword,
                OwnerUserIdentifier = Context.ConnectionId
            });
            _logger.LogInformation("Created room {roomId} {roomName}", roomId, roomName);

            var token = _jwtManager.GenerateJwtToken(roomId);
            return Clients.Caller.SendAsync(QuizHubMethods.RoomCreated, token);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
