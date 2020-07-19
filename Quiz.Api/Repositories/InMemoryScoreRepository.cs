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
        private Dictionary<int, Dictionary<string, (int, TimeSpan)>> _buzzTimesAndPositions = new Dictionary<int, Dictionary<string, (int, TimeSpan)>>();

        // Stores rooms, with dictionary of user ids to scores
        private Dictionary<int, Dictionary<string, int>> _scores = new Dictionary<int, Dictionary<string, int>>();

        // Stores the current time for the room
        private Dictionary<int, Stopwatch> _stopwatches = new Dictionary<int, Stopwatch>();

        public BuzzResult RegisterBuzz(int roomId, string userId)
        {
            int position;
            TimeSpan delay;

            if (!_buzzTimesAndPositions.ContainsKey(roomId))
            {
                _buzzTimesAndPositions.Add(roomId, new Dictionary<string, (int, TimeSpan)>());

                CreateStopwatchForRoom(roomId);

                position = 1;
                delay = TimeSpan.Zero;
            } else
            {
                position = _buzzTimesAndPositions[roomId].Values.Max(score => score.Item1) + 1;
                delay = _stopwatches[roomId].Elapsed;
            }

            if (_buzzTimesAndPositions[roomId].ContainsKey(userId))
            {
                position = _buzzTimesAndPositions[roomId][userId].Item1;
                delay = _buzzTimesAndPositions[roomId][userId].Item2;
            }

            _buzzTimesAndPositions[roomId].Add(userId, (position, delay));

            return new BuzzResult
            {
                Position = position,
                Delay = delay
            };
        }

        public void ResetTimesForRoom(int roomId)
        {
            _buzzTimesAndPositions.Remove(roomId);
            _stopwatches.Remove(roomId);
        }

        public int IncrementUserScore(string userId, int roomId)
        {
            InitializePlayerScoreIfNotExists(userId, roomId);

            _scores[roomId][userId] += 1;
            return _scores[roomId][userId];
        }

        public int DecrementUserScore(string userId, int roomId)
        {
            InitializePlayerScoreIfNotExists(userId, roomId);

            _scores[roomId][userId] -= 1;
            return _scores[roomId][userId];
        }

        public int GetUserScore(string userId, int roomId)
        {
            InitializePlayerScoreIfNotExists(userId, roomId);

            return _scores[roomId][userId];
        }

        public Dictionary<string, int> GetUserScores(int roomId)
        {
            return _scores.GetValueOrDefault(roomId, new Dictionary<string, int>());
        }

        private void InitializePlayerScoreIfNotExists(string userId, int roomId)
        {
            if (!_scores.ContainsKey(roomId))
            {
                _scores[roomId] = new Dictionary<string, int>();
            }

            if (!_scores[roomId].ContainsKey(userId))
            {
                _scores[roomId].Add(userId, 0);
            }
        }

        private void CreateStopwatchForRoom(int roomId)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            _stopwatches.Add(roomId, stopwatch);
        }
    }
}
