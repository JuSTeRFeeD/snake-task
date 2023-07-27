using Unity.Mathematics;

namespace Project.Utilities
{
    public static class BoardUtils
    {
        public const float GridCellSize = 1.25f;
        
        public static int2 GetCellPos(float3 worldPos)
        {
            return new int2(
                (int)(worldPos.x / GridCellSize),
                (int)(worldPos.z / GridCellSize)
            );
        }

        public static float3 GetWorldPosByCellPos(int2 cellPos)
        {
            return new float3(
                cellPos.x * GridCellSize + GridCellSize / 2,
                0,
                cellPos.y * GridCellSize + GridCellSize / 2
                );
        }
    }
}