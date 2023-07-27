using ME.ECS;

namespace Project.Features.Board.Components
{
    public struct SpawnApple : IComponent
    {
    }
    
    public struct Food : IComponent
    {
        public int increaseSnakeSize;
    }

    public struct ToSpawnFoodTimer : IComponent
    {
        public float value;
    }

    public struct ToDespawnTime : IComponent
    {
        public float value;
    }

    public struct DespawnTimer : IComponent
    {
        public float value;
    }
}