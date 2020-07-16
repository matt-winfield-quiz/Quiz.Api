using System.Collections.Generic;

namespace Quiz.Api.Models.Display
{
    public class RoomDisplayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<User> UsersInRoom { get; set; }

        public RoomDisplayModel()
        {
            UsersInRoom = new List<User>();
        }
    }
}
