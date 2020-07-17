using Quiz.Api.Models.Internal;

namespace Quiz.Api.Repositories.Interfaces
{
    public interface IScoreRepository
    {
        BuzzResult RegisterBuzz(int roomId, string userId);
        void ResetScoresForRoom(int roomId);
    }
}
