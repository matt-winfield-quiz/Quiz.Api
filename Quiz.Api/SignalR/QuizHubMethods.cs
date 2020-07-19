namespace Quiz.Api.SignalR
{
    public static class QuizHubMethods
    {
        public const string RoomCreated = "RoomCreated";
        public const string RoomCreateSuccess = "RoomCreateSuccess";
        public const string BuzzerPressed = "BuzzerPressed";
        public const string BuzzerPressSuccess = "BuzzerPressSuccess";
        public const string UserJoinedRoom = "UserJoinedRoom";
        public const string UserJoinRoomSuccess = "UserJoinRoomSuccess";
        public const string UserJoinRoomFail = "UserJoinRoomFail";
        public const string UserLeftRoom = "UserLeftRoom";
        public const string UserUpdatedName = "UserUpdatedName";
        public const string ScoresCleared = "ScoresCleared";
        public const string RoomClosed = "RoomClosed";
        public const string InvalidJwtToken = "InvalidJwtToken";
    }
}
