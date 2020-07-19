using Quiz.Api.Models.Internal;
using System.Collections.Generic;

namespace Quiz.Api.Repositories.Interfaces
{
    public interface IScoreRepository
    {
        BuzzResult RegisterBuzz(int roomId, string userId);
        void ResetTimesForRoom(int roomId);
        int IncrementUserScore(string userId, int roomId);
        int DecrementUserScore(string userId, int roomId);
        int GetUserScore(string userId, int roomId);
        Dictionary<string, int> GetUserScores(int roomId);
    }
}
