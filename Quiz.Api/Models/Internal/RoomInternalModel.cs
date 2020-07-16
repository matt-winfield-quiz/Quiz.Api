using Quiz.Api.Models.Display;
using System.Collections.Generic;

namespace Quiz.Api.Models.Internal
{
    public class RoomInternalModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string OwnerUserIdentifier { get; set; }
        public List<User> UsersInRoom { get; set; }

        public RoomInternalModel()
        {
            UsersInRoom = new List<User>();
        }

        public RoomDisplayModel ToDisplayModel() => new RoomDisplayModel()
            {
                Id = this.Id,
                Name = this.Name,
                UsersInRoom = this.UsersInRoom
            };
    }
}
