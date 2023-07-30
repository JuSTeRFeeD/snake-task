using ME.ECS;
using Project.Components;
using Project.Features.Board;
using Project.Features.Board.Components;
using Project.Features.Snake.Components;
using Project.Utilities;
using UnityEngine;

namespace Project.Features.Snake.Systems
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class SnakeMoveSystem : ISystemFilter
    {
        private BoardFeature boardFeature;
        private SnakeFeature feature;

        private const float PrepareToMoveSeconds = .15f;
        public const float MoveSeconds = .15f;

        public World world { get; set; }

        void ISystemBase.OnConstruct()
        {
            this.GetFeature(out feature);
            this.GetFeature(out boardFeature);
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
                .WithoutShared<GamePaused>()
                .Push();
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            ref var prepareToMoveTime = ref entity.Get<PrepareToMoveTime>();

            if (prepareToMoveTime.value > 0)
            {
                entity.Remove<SnakePartsUpdateEvent>();
                prepareToMoveTime.value -= deltaTime;
                return;
            }

            ref var moveTime = ref entity.Get<MoveTime>();
            ref var targetPosition = ref entity.Get<TargetPosition>();
            ref var startMovePosition = ref entity.Get<StartMovePosition>();
            ref var prevPositionInfo = ref entity.Get<PrevPositionInfo>();

            if (!entity.Has<IsMove>())
            {
                ref readonly var positionOnBoard = ref entity.Read<PositionOnBoard>().value;
                ref readonly var moveDirection = ref entity.Read<MoveDirection>().value;

                prevPositionInfo.direction = moveDirection;
                
                var targetCellPos = positionOnBoard + moveDirection;
                targetPosition.value = BoardUtils.GetWorldPosByCellPos(targetCellPos);

                // Check to teleport
                if (!boardFeature.CheckPosOnBoard(targetCellPos))
                {
                    var boardSize = boardFeature.BoardSize;
                    if (targetCellPos.x >= boardSize.sizeX) targetCellPos.x = 0;
                    else if (targetCellPos.x < 0) targetCellPos.x = boardSize.sizeX - 1;
                    else if (targetCellPos.y >= boardSize.sizeY) targetCellPos.y = 0;
                    else if (targetCellPos.y < 0) targetCellPos.y = boardSize.sizeY - 1;
                    targetPosition.value = BoardUtils.GetWorldPosByCellPos(targetCellPos);

                    EndMove(entity, ref startMovePosition, ref targetPosition,
                        ref prepareToMoveTime, ref moveTime, ref prevPositionInfo);

                    return;
                }
            }

            moveTime.value += deltaTime;
            
            if (moveTime.value < MoveSeconds)
            {
                var t = moveTime.value / MoveSeconds;
                var newPos = Vector3.Lerp(startMovePosition.value, targetPosition.value, t);
                entity.SetPosition(newPos);
                entity.Get<ChangePositionEvent>();
                entity.Get<IsMove>();
            }
            else
            {
                EndMove(entity, ref startMovePosition, ref targetPosition, ref prepareToMoveTime, ref moveTime,
                    ref prevPositionInfo);
            }
        }

        private static void EndMove(in Entity entity, ref StartMovePosition startMovePosition, ref TargetPosition targetPosition,
            ref PrepareToMoveTime prepareToMoveTime, ref MoveTime moveTime, ref PrevPositionInfo prevPositionInfo)
        {
            prepareToMoveTime.value = PrepareToMoveSeconds;
            moveTime.value = 0;

            prevPositionInfo.position = startMovePosition.value;

            entity.SetPosition(targetPosition.value);
            startMovePosition.value = targetPosition.value;

            entity.Remove<IsMove>();
            entity.Get<SnakePartsUpdateEvent>();
            entity.Get<ChangePositionEvent>();
            entity.Get<ChangePositionEvent>();
        }
    }
}