using Quiz.Api.Models.Internal;
using System.Collections.ObjectModel;

namespace Quiz.Api.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        public ReadOnlyCollection<RoomInternalModel> GetRooms();
        public int AddRoom(RoomInternalModel room);
    }
}
