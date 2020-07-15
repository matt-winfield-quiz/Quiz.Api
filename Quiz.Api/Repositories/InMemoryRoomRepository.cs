using Quiz.Api.Models.Internal;
using Quiz.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
    }
}
