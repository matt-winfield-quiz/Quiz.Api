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
        private JwtManager _jwtManager;
        private ILogger<QuizHub> _logger;

        private const string RoomGroupNamePrefix = "Room";

        public QuizHub(IRoomRepository roomRepository, IUserRepository userRepository, JwtManager jwtManager, ILogger<QuizHub> logger)
        {
            _roomRepository = roomRepository;
            _userRepository = userRepository;
            _jwtManager = jwtManager;
            _logger = logger;
        }

        public async Task CreateRoom(string roomName, string roomPassword)
        {
            var roomId = _roomRepository.AddRoom(new RoomInternalModel
            {
                Name = roomName,
                Password = roomPassword,
                OwnerUserIdentifier = Context.ConnectionId
            });
            _logger.LogInformation("Created room {roomId} {roomName}", roomId, roomName);

            var token = _jwtManager.GenerateJwtToken(roomId);
            await Clients.Caller.SendAsync(QuizHubMethods.RoomCreated, token);
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

            var user = _userRepository.GetUser(Context.ConnectionId);

            await Clients.Group(GetRoomGroupName(roomId)).SendAsync(QuizHubMethods.BuzzerPressed, roomId, user);
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
