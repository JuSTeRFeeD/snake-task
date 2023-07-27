using ME.ECS;
using Project.Features.Board.Components;
using Project.Utilities;
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
    public sealed class SnakeMoveSystem : ISystemFilter
    {
        private SnakeFeature feature;

        private const float PrepareToMoveSeconds = .15f;
        public const float MoveSeconds = .15f;

        public World world { get; set; }

        void ISystemBase.OnConstruct()
        {
            this.GetFeature(out feature);
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
            return Filter.Create("Filter-SnakeMoveSystem")
                .With<IsSnakeHead>()
                .With<MoveDirection>()
                .With<TargetPosition>()
                .With<StartMovePosition>()
                .Push();
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            ref var prepareToMoveTime = ref entity.Get<PrepareToMoveTime>();

            if (prepareToMoveTime.value > 0)
            {
                prepareToMoveTime.value -= deltaTime;
                return;
            }

            ref var moveTime = ref entity.Get<MoveTime>();
            ref var targetPosition = ref entity.Get<TargetPosition>();
            ref var startMovePosition = ref entity.Get<StartMovePosition>();

            if (!entity.Has<IsMove>())
            {
                ref readonly var positionOnBoard = ref entity.Get<PositionOnBoard>().value;
                ref readonly var moveDirection = ref entity.Get<MoveDirection>().value;
                
                targetPosition.value = BoardUtils.GetWorldPosByCellPos(positionOnBoard + moveDirection);
            }

            moveTime.value += deltaTime;
            
            ref var prevPositionInfo = ref entity.Get<PrevPositionInfo>();

            if (moveTime.value < MoveSeconds)
            {
                var t = moveTime.value / MoveSeconds;
                var newPos = Vector3.Lerp(startMovePosition.value, targetPosition.value, t);
                entity.SetPosition(newPos);
                entity.Get<IsMove>();
             
                var dir = ((Vector3)targetPosition.value - (Vector3)startMovePosition.value).normalized;
                prevPositionInfo.direction = new int2((int)dir.x, (int)dir.z);
            }
            else
            {
                prepareToMoveTime.value = PrepareToMoveSeconds;
                moveTime.value = 0;
                
                prevPositionInfo.position = startMovePosition.value;

                entity.SetPosition(targetPosition.value);
                startMovePosition.value = targetPosition.value;

                entity.Remove<IsMove>();
                entity.Get<ChangePositionEvent>();
                
                world.AddMarker(new SnakePartsUpdateMarker()); // TODO: delete
            }
        }
    }
}