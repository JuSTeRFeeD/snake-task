using ME.ECS;
using Unity.Mathematics;

namespace Project.Features.Board
{
    public class BoardCell
    {
        public Entity entity = default;
        public float3 worldPosition;
    }
}