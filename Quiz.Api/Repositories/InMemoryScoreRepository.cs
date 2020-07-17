using Quiz.Api.Models.Internal;
using Quiz.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Quiz.Api.Repositories
{
    public class InMemoryScoreRepository : IScoreRepository
    {
        // Stores rooms, with dictionary of user ids to time to press buzzer (0ms is first press) and position in room
        private Dictionary<int, Dictionary<string, (int, TimeSpan)>> _scores = new Dictionary<int, Dictionary<string, (int, TimeSpan)>>();

        // Stores the current time for the room
        private Dictionary<int, Stopwatch> _stopwatches = new Dictionary<int, Stopwatch>();

        public BuzzResult RegisterBuzz(int roomId, string userId)
        {
            int position;
            TimeSpan delay;

            if (!_scores.ContainsKey(roomId))
            {
                _scores.Add(roomId, new Dictionary<string, (int, TimeSpan)>());

                CreateStopwatchForRoom(roomId);

                position = 1;
                delay = TimeSpan.Zero;
            } else
            {
                position = _scores[roomId].Values.Max(score => score.Item1) + 1;
                delay = _stopwatches[roomId].Elapsed;
            }

            if (_scores[roomId].ContainsKey(userId))
            {
                position = _scores[roomId][userId].Item1;
                delay = _scores[roomId][userId].Item2;
            }

            _scores[roomId].Add(userId, (position, delay));

            return new BuzzResult
            {
                Position = position,
                Delay = delay
            };
        }

        public void ResetScoresForRoom(int roomId)
        {
            _scores.Remove(roomId);
            _stopwatches.Remove(roomId);
        }

        private void CreateStopwatchForRoom(int roomId)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            _stopwatches.Add(roomId, stopwatch);
        }
    }
}
