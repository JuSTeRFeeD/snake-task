// ReSharper disable All
namespace Project.Network
{
    public class Message
    {
        public string type;
    }

    public class StartGameMessage
    {
        public readonly string type = "create-game";
    }

    public class GameCreatedMessage
    {
        public string type; // "game-created"
        public Payload payload;
        public class Payload
        {
            public string clientAddress; // "176.53.197.134:42710"
            public string startAt; // "1687359171872"
            public string finishAt; // null
            public int id; // 1
            public string collectedApples; // 0
            public string snakeLength; // 0
            public string created_at; // "2023-06-21T14:52:51.887Z"
            public string updated_at; // "2023-06-21T14:52:51.887Z"  
        }
    }

    public class CollectAppleMessage
    {
        public readonly string type = "collect-apple";
        public Payload payload;
        public class Payload
        {
            public int appleCount; // 2
            public int snakeLength; // 3
            public int game_id; // 1
        }
    }
    

    public class GameEndMessage
    {
        public readonly string type = "end-game";
        public Payload payload;
        public class Payload
        {
            public int game_id;
        }
    }
    
    public class GameEndedMessage
    {
        public string type; // "game-ended"
        public Payload payload;
        public class Payload
        {
            public int id; // 1
            public string startAt; // "1687359171872"
            public string finishAt; // 16873591758721
            public int collectedApples; // 2
            public int snakeLength; // 3
            public string created_at; // "2023-06-21T14:52:51.887Z"
            public string updated_at; // "2023-06-21T14:52:51.887Z"  
        }
    }
}