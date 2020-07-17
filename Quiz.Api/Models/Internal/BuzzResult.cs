using System;

namespace Quiz.Api.Models.Internal
{
    public class BuzzResult
    {
        public bool IsFirstBuzz { get => Position == 1; }
        public int Position { get; set; }
        public TimeSpan Delay { get; set; }
    }
}
