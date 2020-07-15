using Quiz.Api.Models.Display;

namespace Quiz.Api.Models.Internal
{
    public class RoomInternalModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string OwnerUserIdentifier { get; set; }

        public RoomDisplayModel ToDisplayModel() => new RoomDisplayModel()
            {
                Id = this.Id,
                Name = this.Name
            };
    }
}
