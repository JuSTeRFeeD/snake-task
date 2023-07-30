using ME.ECS;

namespace Project.Features.Board.Components
{
    public enum FoodType
    {
        Apple,
        Banana
    }
    
    public struct SpawnFoodEvent : IComponent
    {
        public bool isInitSpawn;
    }
    
    public struct Food : IComponent
    {
        public FoodType foodType;
        public int increaseSnakeSize;
    }
}