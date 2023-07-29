using System;
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
    public sealed class SnakePartsMoveSystem : ISystemFilter
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
            
            ref readonly var positionOnBoard = ref entity.Get<PositionOnBoard>().value;
            ref var startMovePosition = ref entity.Get<StartMovePosition>();
            ref var prevPositionInfo = ref entity.Get<PrevPositionInfo>();

            var snakeHead = feature.GetSnakeHead();

            if (snakeHead.Has<SnakePartsUpdateEvent>())
            {
                entity.SetPosition(targetPosition.value);
                entity.Get<PrevPositionInfo>().position = startMovePosition.value;
                entity.Get<ChangePositionEvent>();
            }
            
            if (!snakeHead.Has<IsMove>())
            {
                entity.SetPosition(nextSnakePart.Get<PrevPositionInfo>().position);
                startMovePosition.value = entity.GetPosition();
                return;
            }
            
            var dir = ((Vector3)targetPosition.value - (Vector3)startMovePosition.value).normalized;
            prevPositionInfo.direction = new int2((int)dir.x, (int)dir.z);

            // Check to teleport
            {
                var targetCellPos = BoardUtils.GetCellPos(targetPosition.value);
                var diff = new int2(Math.Abs(targetCellPos.x - positionOnBoard.x),
                    Math.Abs(targetCellPos.y - positionOnBoard.y));
                if (diff.x > 1 || diff.y > 1)
                {
                    entity.SetPosition(targetPosition.value);
                    return;
                }
            }
            
            ref readonly var moveTime = ref snakeHead.Read<MoveTime>().value;
            var t = moveTime / SnakeMoveSystem.MoveSeconds;
            var newPos = Vector3.Lerp(startMovePosition.value, targetPosition.value, t);
            entity.SetPosition(newPos);

            // Check to teleport
            // var checkStart = (Vector3)startMovePosition.value;
            // var checkEnd = (Vector3)targetPosition.value;
            // if ((checkStart - checkEnd).sqrMagnitude > BoardUtils.GridCellSize * 3)
            // {
            //     entity.Get<PrevPositionInfo>().position = startMovePosition.value;
            //     
            //     var dir = ((Vector3)targetPosition.value - (Vector3)startMovePosition.value).normalized;
            //     prevPositionInfo.direction = new int2((int)dir.x, (int)dir.z);
            //     
            //     entity.SetPosition(nextSnakePart.Get<PrevPositionInfo>().position);
            //     entity.Get<ChangePositionEvent>();
            //     return;
            // }

            // ref readonly var moveTime = ref snakeHead.Get<MoveTime>();
            //
            // if (moveTime.value < SnakeMoveSystem.MoveSeconds)
            // {
            //     var t = moveTime.value / SnakeMoveSystem.MoveSeconds;
            //     var newPos = Vector3.Lerp(startMovePosition.value, targetPosition.value, t);
            //     entity.SetPosition(newPos);
            //     
            //     var dir = ((Vector3)targetPosition.value - (Vector3)startMovePosition.value).normalized;
            //     prevPositionInfo.direction = new int2((int)dir.x, (int)dir.z);
            // }
            //
            // entity.Get<ChangePositionEvent>();
        }
    }
}