using ME.ECS;
using Project.Features.Board.Components;
using Unity.Mathematics;
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
    public sealed class SnakePartsMoveSystem : ISystemFilter
    {
        private SnakeFeature feature;

        public World world { get; set; }

        void ISystemBase.OnConstruct()
        {
            this.GetFeature(out this.feature);
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
            return Filter.Create("Filter-SnakePartsMoveSystem")
                .With<SnakeLink>()
                .With<StartMovePosition>()
                .Push();
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            ref readonly var nextSnakePart = ref entity.Get<SnakeLink>().prevPart;
            ref readonly var targetPosition = ref nextSnakePart.Get<StartMovePosition>();
            ref var startMovePosition = ref entity.Get<StartMovePosition>();

            var snakeHead = feature.GetSnakeHead();

            if (world.HasMarker<SnakePartsUpdateMarker>())
            {
                entity.SetPosition(targetPosition.value); // here
                entity.Get<PrevPositionInfo>().position = startMovePosition.value;
                entity.Get<ChangePositionEvent>();
            }
            
            if (!snakeHead.Has<IsMove>())
            {
                entity.SetPosition(nextSnakePart.Get<PrevPositionInfo>().position);
                startMovePosition.value = entity.GetPosition();
                return;
            }

            ref readonly var moveTime = ref snakeHead.Get<MoveTime>();
            ref var prevPositionInfo = ref entity.Get<PrevPositionInfo>();

            if (moveTime.value < SnakeMoveSystem.MoveSeconds)
            {
                var t = moveTime.value / SnakeMoveSystem.MoveSeconds;
                var newPos = Vector3.Lerp(startMovePosition.value, targetPosition.value, t);
                entity.SetPosition(newPos);
                
                var dir = ((Vector3)targetPosition.value - (Vector3)startMovePosition.value).normalized;
                prevPositionInfo.direction = new int2((int)dir.x, (int)dir.z);
                Debug.Log($"Part new dir {((int)dir.x, (int)dir.z)}");
            }
        }
    }
}