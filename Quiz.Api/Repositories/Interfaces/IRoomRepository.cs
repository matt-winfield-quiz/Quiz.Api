using Quiz.Api.Models.Display;
using Quiz.Api.Models.Internal;
using System.Collections.ObjectModel;

namespace Quiz.Api.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        ReadOnlyCollection<RoomInternalModel> GetRooms();
        int AddRoom(RoomInternalModel room);
        RoomInternalModel GetRoom(int roomId);
        void AddUserToRoom(User user, int roomId);
        void RemoveUserFromAllRooms(User user);
        void DeleteRoom(int roomId);
    }
}
