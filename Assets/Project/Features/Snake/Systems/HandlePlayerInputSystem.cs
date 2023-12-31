﻿using ME.ECS;

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
    public sealed class HandlePlayerInputSystem : ISystem, IAdvanceTick, IUpdate
    {
        private SnakeFeature feature;

        public World world { get; set; }

        void ISystemBase.OnConstruct()
        {
            this.GetFeature(out feature);
        }
        
        void ISystemBase.OnDeconstruct()
        {
        }

        void IAdvanceTick.AdvanceTick(in float deltaTime)
        {
        }

        void IUpdate.Update(in float deltaTime)
        {
            if (!world.GetMarker(out PlayerMoveInputMarker input)) return;

            var snakeHead = feature.GetSnakeHead();
            if (snakeHead.IsEmpty()) return;

            ref var prevDir = ref snakeHead.Get<PrevPositionInfo>().direction;
            if (prevDir.x == -input.value.x) return;
            if (prevDir.y == -input.value.y) return;
            snakeHead.Get<MoveDirection>().value = input.value;
        }
    }
}