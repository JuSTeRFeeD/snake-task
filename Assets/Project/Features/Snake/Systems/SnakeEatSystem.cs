using ME.ECS;
using Project.Components;
using Project.Features.Board.Components;
using Project.Features.Destroy.Components;
using Project.Features.GameState;
using Project.Features.Snake.Components;
using Unity.Collections;

namespace Project.Features.Snake.Systems
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class SnakeEatSystem : ISystemFilter
    {
        private GameStateFeature gameStateFeature;
        private SnakeFeature feature;

        private Filter foodFilter;

        private RPCId collectAppleRpcId;

        public World world { get; set; }

        void ISystemBase.OnConstruct()
        {
            this.GetFeature(out feature);
            this.GetFeature(out gameStateFeature);

            Filter.Create("Filter-SnakeEatSystem")
                .With<Food>()
                .With<PositionOnBoard>()
                .Push(ref foodFilter);
        }

        void ISystemBase.OnDeconstruct()
        {
        }

#if !CSHARP_8_OR_NEWER
        bool ISystemFilter.jobs => false;
        int ISystemFilter.jobsBatchCount => 64;
#endif
        Filter ISystemFilter.filter { get; set; }

        Filter ISystemFilter.CreateFilter()
        {
            return Filter.Create("Filter-SnakeEatSystem")
                .With<IsSnakeHead>()
                .With<PositionOnBoard>()
                .With<CollisionWithEntity>()
                .WithShared<GameInfo>()
                .WithoutShared<GamePaused>()
                .Push();
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            ref var collisionWith = ref entity.Get<CollisionWithEntity>().entity;
            
            if (collisionWith.Has<IsSnakePart>())
            {
                gameStateFeature.EndGame();
                return;
            }

            if (collisionWith.Has<Food>())
            {
                feature.foodEaten.Execute();
                
                ref var gameInfo = ref world.GetSharedData<GameInfo>();
                ref var food = ref collisionWith.Get<Food>();
                collisionWith.Get<ToDespawn>();

                if (food.foodType == FoodType.Apple)
                {
                    gameInfo.appleCount++;
                }
                
                var group = world.AddEntities(food.increaseSnakeSize, Allocator.Temp, true);
                group.Set(new SpawnSnakePartEvent());

                SpawnFood();
            }

            entity.Remove<CollisionWithEntity>();
        }

        private void SpawnFood()
        {
            var apple = world.AddEntity();
            apple.Set<SpawnFoodEvent>();
        }
    }
}