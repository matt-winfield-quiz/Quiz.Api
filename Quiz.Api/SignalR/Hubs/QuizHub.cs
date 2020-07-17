﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Quiz.Api.Jwt;
using Quiz.Api.Models.Display;
using Quiz.Api.Models.Internal;
using Quiz.Api.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace Quiz.Api.SignalR.Hubs
{
    public class QuizHub : Hub
    {
        private IRoomRepository _roomRepository;
        private IUserRepository _userRepository;
        private IScoreRepository _scoreRepository;
        private JwtManager _jwtManager;
        private ILogger<QuizHub> _logger;

        private const string RoomGroupNamePrefix = "Room";

        public QuizHub(IRoomRepository roomRepository, IUserRepository userRepository, IScoreRepository scoreRepository, JwtManager jwtManager, ILogger<QuizHub> logger)
        {
            _roomRepository = roomRepository;
            _userRepository = userRepository;
            _scoreRepository = scoreRepository;
            _jwtManager = jwtManager;
            _logger = logger;
        }

        public async Task CreateRoom(string roomName, string roomPassword)
        {
            var roomInternalModel = new RoomInternalModel
            {
                Name = roomName,
                Password = roomPassword,
                OwnerUserIdentifier = Context.ConnectionId
            };

            var roomId = _roomRepository.AddRoom(roomInternalModel);
            _logger.LogInformation("Created room {roomId} {roomName}", roomId, roomName);

            var token = _jwtManager.GenerateJwtToken(roomId);
            await Clients.Caller.SendAsync(QuizHubMethods.RoomCreateSuccess, token, roomId);
            await Clients.All.SendAsync(QuizHubMethods.RoomCreated, roomInternalModel.ToDisplayModel());
        }

        public async Task JoinRoom(int roomId, string name)
        {
            var roomGroupName = GetRoomGroupName(roomId);

            var newUser = new User
            {
                Id = Context.ConnectionId,
                Name = name
            };

            _userRepository.CreateUser(newUser);

            _logger.LogInformation("User {name} ({connectionId}) joined room {roomId}", name, Context.ConnectionId, roomId);

            try
            {
                _roomRepository.AddUserToRoom(newUser, roomId);

                await Groups.AddToGroupAsync(Context.ConnectionId, roomGroupName);

                await Clients.Caller.SendAsync(QuizHubMethods.UserJoinRoomSuccess);
                await Clients.Group(roomGroupName).SendAsync(QuizHubMethods.UserJoinedRoom, newUser);
            } catch (ArgumentException)
            {
                _logger.LogError("Failed to add user {user} to room {roomId}", newUser, roomId);
                await Clients.Caller.SendAsync(QuizHubMethods.UserJoinRoomFail);
            }
        }

        public async Task Buzz(int roomId)
        {
            _logger.LogInformation("Buzz triggered by {connectionId}", Context.ConnectionId);

            var buzzResult = _scoreRepository.RegisterBuzz(roomId, Context.ConnectionId);

            var user = _userRepository.GetUser(Context.ConnectionId);

            await Clients.Caller.SendAsync(QuizHubMethods.BuzzerPressSuccess, buzzResult);
            await Clients.Group(GetRoomGroupName(roomId)).SendAsync(QuizHubMethods.BuzzerPressed, user, buzzResult);
        }

        public async Task ClearScores(int roomId)
        {
            _logger.LogInformation("Scores cleared for room {roomId} by {connectionId}", roomId, Context.ConnectionId);
            _scoreRepository.ResetScoresForRoom(roomId);

            await Clients.Group(GetRoomGroupName(roomId)).SendAsync(QuizHubMethods.ScoresCleared);
        }

        public async Task UpdateUsername(string newUsername)
        {
            _logger.LogInformation("Updating username for {connectionId} to {newUsername}", Context.ConnectionId, newUsername);

            var user = _userRepository.GetUser(Context.ConnectionId);
            user.Name = newUsername;

            await Clients.All.SendAsync(QuizHubMethods.UserUpdatedName, Context.ConnectionId, newUsername);
        }

        public async Task RemoveRoom(int roomId)
        {
            _logger.LogInformation("Removing room {roomId}", roomId);

            _roomRepository.DeleteRoom(roomId);

            await Clients.All.SendAsync(QuizHubMethods.RoomClosed, roomId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = _userRepository.GetUser(Context.ConnectionId);
            if (user != null)
            {
                _roomRepository.RemoveUserFromAllRooms(user);
                _userRepository.RemoveUser(user.Id);
                _logger.LogInformation("User {username} ({userId}) left", user.Name, user.Id);
                await Clients.All.SendAsync(QuizHubMethods.UserLeftRoom, Context.ConnectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        private string GetRoomGroupName(int roomId)
        {
            return $"{RoomGroupNamePrefix}{roomId.ToString()}";
        }
    }
}
