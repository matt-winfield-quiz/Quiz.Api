using Quiz.Api.Models.Display;
using Quiz.Api.Models.Internal;
using Quiz.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Quiz.Api.Repositories
{
    public class InMemoryRoomRepository : IRoomRepository
    {
        private List<RoomInternalModel> _rooms = new List<RoomInternalModel>();
        private int _nextRoomId = 1;

        public ReadOnlyCollection<RoomInternalModel> GetRooms()
        {
            return _rooms.AsReadOnly();
        }

        public int AddRoom(RoomInternalModel room)
        {
            room.Id = _nextRoomId++;
            _rooms.Add(room);
            return room.Id;
        }

        public RoomInternalModel GetRoom(int roomId)
        {
            return _rooms.FirstOrDefault(room => room.Id == roomId);
        }

        public void AddUserToRoom(User user, int roomId)
        {
            var room = _rooms.FirstOrDefault(room => room.Id == roomId);

            if (room == null)
            {
                throw new ArgumentException($"{roomId} not found!");
            }

            room.UsersInRoom.Add(user);
            Console.WriteLine(room.ToString());
        }
    }
}
