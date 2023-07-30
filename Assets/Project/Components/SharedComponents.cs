using ME.ECS;

namespace Project.Components
{
    public struct GamePaused : IComponent
    {
    }

    public struct GameInfo : IComponent
    {
        public int gameId;
        public int appleCount;
        public int snakeLength;
    }

    public struct GameEndInfo : IComponent
    {
        public GameInfo gameInfo;
        public int playedTime;
    }
}