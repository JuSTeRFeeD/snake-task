using ME.ECS;
using ME.ECS.DataConfigs;
using Unity.Collections;
using UnityEngine;

namespace Project.Features.Snake
{
    using Components;
    using Modules;
    using Systems;
    using Features;
    using Markers;
    using Snake.Components;
    using Snake.Modules;
    using Snake.Systems;
    using Snake.Markers;

    namespace Snake.Components
    {
    }

    namespace Snake.Modules
    {
    }

    namespace Snake.Systems
    {
    }

    namespace Snake.Markers
    {
    }

#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class SnakeFeature : Feature
    {
        private Filter snakeHeadFilter;

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

            var tailLen = initSnakeConfig.Get<SnakeStartSize>().value;
            var startPos = initSnakeConfig.Get<SnakeStartPosition>().startPosition;
            
            var group = world.AddEntities(tailLen, Allocator.Temp, copyMode: true);
            group.Set(new SpawnSnakePart());
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