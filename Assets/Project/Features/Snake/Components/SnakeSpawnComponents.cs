using ME.ECS;
using Unity.Mathematics;

namespace Project.Features.Snake.Components
{
    public struct SnakeStartSize : IComponent
    {
        public int value;
    }

    public struct SnakeStartPosition : IComponent
    {
        public int2 startPosition;
    }

    public struct SpawnSnakePartEvent : IComponent
    {
    }
}