using ME.ECS;
using ME.ECS.DataConfigs;
using Project.Features.Snake.Components;
using Project.Features.Snake.Systems;
using Unity.Collections;
using UnityEngine;

namespace Project.Features.Snake
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class SnakeFeature : Feature
    {
        private Filter snakeHeadFilter;
        
        public GlobalEvent foodEaten;
        
        protected override void OnConstruct()
        {
            AddSystem<SnakeEatSystem>();
            AddSystem<HandlePlayerInputSystem>();
            AddSystem<SnakeSpawnSystem>();
            AddSystem<SnakeMoveSystem>();
            AddSystem<SnakePartsMoveSystem>();
            
            Filter.Create("Snake-Head-Filter")
                .With<IsSnakeHead>()
                .Push(ref snakeHeadFilter);

            SpawnSnake();
        }

        private void SpawnSnake()
        {
            var initSnakeConfig = Resources.Load<DataConfig>("SnakeInitConfig");

            var initLenght = initSnakeConfig.Get<SnakeStartSize>().value;
            var startPos = initSnakeConfig.Get<SnakeStartPosition>().startPosition;
            
            var group = world.AddEntities(initLenght, Allocator.Temp, copyMode: true);
            group.Set(new SpawnSnakePartEvent());
            group.Set(new IsInitPart());
            group.Set(new SnakeStartPosition()
            {
                startPosition = startPos 
            });
        }

        protected override void OnDeconstruct()
        {
        }
        
        public Entity GetSnakeHead()
        {
            foreach (var e in snakeHeadFilter)
            {
                return e;
            }

            return Entity.Empty;
        }
    }
}