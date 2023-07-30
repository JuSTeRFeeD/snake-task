using ME.ECS;

namespace Project.Markers
{
    public struct GameEndedMarker : IMarker
    {
        public int playedTime;
        public int snakeLength;
        public int collectedApples;
    }
}