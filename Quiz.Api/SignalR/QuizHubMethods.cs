namespace Quiz.Api.SignalR
{
    public static class QuizHubMethods
    {
        public const string RoomCreated = "RoomCreated";
        public const string BuzzerPressed = "BuzzerPressed";
        public const string UserJoinedRoom = "UserJoinedRoom";
        public const string UserJoinRoomSuccess = "UserJoinRoomSuccess";
        public const string UserJoinRoomFail = "UserJoinRoomFail";
    }
}
