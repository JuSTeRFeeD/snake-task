using ME.ECS;
using Unity.Mathematics;

namespace Project.Features.Board.Components {

    public struct BoardSize : IComponent
    {
        public int sizeX;
        public int sizeY;
    }

    public struct ChangePositionEvent : IComponent
    {
    }

    public struct PositionOnBoard : IComponent
    {
        public int2 value;
    }
}