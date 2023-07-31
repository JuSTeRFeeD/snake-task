using System;
using ME.ECS;
using Project.Components;
using Project.Features.Board.Components;
using Project.Features.Snake.Components;
using Project.Utilities;
using Unity.Mathematics;
using UnityEngine;

namespace Project.Features.Snake.Systems
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class SnakePartsMoveSystem : ISystemFilter
    {
        private SnakeFeature feature;

        private const int DiffToTeleport = 2;

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
                .With<IsSnakePart>()
                .With<SnakeLink>()
                .With<StartMovePosition>()
                .With<PositionOnBoard>()
                .With<StartMovePosition>()
                .With<PrevPositionInfo>()
                .WithoutShared<GamePaused>()
                .Push();
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            ref readonly var nextSnakePart = ref entity.Read<SnakeLink>().prevPart;
            ref readonly var targetPosition = ref nextSnakePart.Read<StartMovePosition>();
            
            ref readonly var positionOnBoard = ref entity.Read<PositionOnBoard>().value;
            ref var startMovePosition = ref entity.Get<StartMovePosition>();
            ref var prevPositionInfo = ref entity.Get<PrevPositionInfo>();

            var snakeHead = feature.GetSnakeHead();

            if (snakeHead.Has<SnakePartsUpdateEvent>())
            {
                entity.SetPosition(targetPosition.value);
                prevPositionInfo.position = startMovePosition.value;
                prevPositionInfo.direction = nextSnakePart.Read<PrevPositionInfo>().direction;
                entity.Set<ChangePositionEvent>();
            }
            
            if (!snakeHead.Has<IsMove>())
            {
                entity.SetPosition(nextSnakePart.Read<PrevPositionInfo>().position);
                startMovePosition.value = entity.GetPosition();
                return;
            }
            
            // Check to teleport
            var targetCellPos = BoardUtils.GetCellPos(targetPosition.value);
            var diff = new int2(Math.Abs(targetCellPos.x - positionOnBoard.x),
                Math.Abs(targetCellPos.y - positionOnBoard.y));
            if (diff.x > DiffToTeleport || diff.y > DiffToTeleport)
            {
                entity.SetPosition(targetPosition.value);
                return;
            }
            
            // Smooth move
            ref readonly var moveTime = ref snakeHead.Read<MoveTime>().value;
            var t = moveTime / SnakeMoveSystem.MoveSeconds;
            var newPos = Vector3.Lerp(startMovePosition.value, targetPosition.value, t);
            entity.SetPosition(newPos);
        }
    }
}