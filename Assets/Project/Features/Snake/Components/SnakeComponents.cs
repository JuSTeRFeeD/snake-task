using ME.ECS;
using Unity.Mathematics;

namespace Project.Features.Snake.Components
{
    public struct IsInitPart : IComponent
    {
    }
    
    public struct IsSnakeHead : IComponent
    {
    }

    public struct IsSnakePart : IComponent
    {
    }

    public struct IsMove : IComponent
    {
    }

    public struct SnakeLink : IComponent
    {
        public Entity prevPart;
    }

    public struct MoveDirection : IComponent
    {
        public int2 value;
    }

    public struct TargetPosition : IComponent
    {
        public float3 value;
    }

    public struct StartMovePosition : IComponent
    {
        public float3 value;
    }

    public struct PrevPositionInfo : IComponent
    {
        public float3 position;
        public int2 direction;
    }

    public struct PrepareToMoveTime : IComponent
    {
        public float value;
    }

    public struct MoveTime : IComponent
    {
        public float value;
    }

    public struct SnakePartsUpdateEvent : IComponent
    {
    }
}