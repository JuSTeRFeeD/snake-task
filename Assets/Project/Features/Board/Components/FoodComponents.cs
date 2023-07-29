using ME.ECS;

namespace Project.Features.Board.Components
{
    public enum FoodType
    {
        Apple,
        Banana
    }
    
    public struct SpawnFood : IComponent
    {
        public FoodType foodType;
    }
    
    public struct Food : IComponent
    {
        public FoodType foodType;
        public int increaseSnakeSize;
    }
}