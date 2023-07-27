﻿using ME.ECS;
using Project.Features.Board;
using Project.Features.Board.Components;
using Unity.Collections;
using UnityEngine;

namespace Project.Features.Snake.Systems
{
#pragma warning disable
    using Project.Components;
    using Project.Modules;
    using Project.Systems;
    using Project.Markers;
    using Components;
    using Modules;
    using Systems;
    using Markers;

#pragma warning restore

#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class SnakeEatSystem : ISystemFilter
    {
        private SnakeFeature feature;
        private BoardFeature boardFeature;

        private Filter foodFilter;

        public World world { get; set; }

        void ISystemBase.OnConstruct()
        {
            this.GetFeature(out feature);
            this.GetFeature(out boardFeature);

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
                .Push();
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            ref var collisionWith = ref entity.Get<CollisionWithEntity>().entity;
            
            if (collisionWith.Has<IsSnakePart>())
            {
                world.RemoveEntity(entity);
                Debug.Log("END GAME");
                return;
            }

            if (collisionWith.Has<Food>())
            {
                ref var food = ref collisionWith.Get<Food>();
                
                var group = world.AddEntities(food.increaseSnakeSize, Allocator.Temp, true);
                group.Set(new SpawnSnakePart());

                world.RemoveEntity(collisionWith);
                
                var e = world.AddEntity();
                e.Get<SpawnApple>();
            }

            entity.Remove<CollisionWithEntity>();
        }
    }
}