﻿using ME.ECS;
using ME.ECS.DataConfigs;
using Project.Components;
using Project.Features.Board.Components;
using Project.Features.Snake.Components;
using Project.Markers;
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
    public sealed class SnakeSpawnSystem : ISystemFilter
    {
        private SnakeFeature feature;

        public World world { get; set; }

        private ViewId snakeHeadViewId;
        private ViewId snakePartViewId;

        private DataConfig snakePartConfig;
        private DataConfig snakeHeadConfig;

        private Entity lastPartOfSnake;

        void ISystemBase.OnConstruct()
        {
            this.GetFeature(out feature);

            snakeHeadConfig = Resources.Load<DataConfig>("SnakeHeadConfig");
            snakePartConfig = Resources.Load<DataConfig>("SnakePartConfig");

            RegisterViews();
        }

        private void RegisterViews()
        {
            var headView = snakeHeadConfig.Read<DataConfigViewReference>().prefabView;
            snakeHeadViewId = world.RegisterViewSource(headView);
            var partView = snakePartConfig.Read<DataConfigViewReference>().prefabView;
            snakePartViewId = world.RegisterViewSource(partView);
        }

        private Entity CreateSnakeHead(ConfigBase config, int2 boardCellPos)
        {
            var entity = CreateEntityWithConfig(config);

            var worldPos = BoardUtils.GetWorldPosByCellPos(boardCellPos);

            var initMoveDir = new int2(0, 1);

            entity.SetPosition(worldPos);

            entity.Set<IsSnakeHead>();
            entity.Set<IsSnakePart>();
            entity.Set(new PositionOnBoard { value = boardCellPos });
            entity.Set(new StartMovePosition { value = worldPos });
            entity.Set(new PrevPositionInfo
            {
                position = BoardUtils.GetWorldPosByCellPos(boardCellPos - initMoveDir),
                direction = initMoveDir,
            });
            entity.Set(new TargetPosition { value = BoardUtils.GetWorldPosByCellPos(boardCellPos + initMoveDir) });
            entity.Set(new MoveDirection { value = initMoveDir });

            entity.InstantiateView(snakeHeadViewId);

            return entity;
        }

        private Entity CreateSnakePart(ConfigBase config, Entity previousPart, bool isInitPart)
        {
            var entity = CreateEntityWithConfig(config);

            ref readonly var prevPartDirection = ref previousPart.Read<PrevPositionInfo>().direction;
            var boardCellPos = previousPart.Read<PositionOnBoard>().value;

            // spawn on last part position if not initializing
            if (isInitPart)
            {
                boardCellPos -= prevPartDirection;
            }

            var worldPos = BoardUtils.GetWorldPosByCellPos(boardCellPos);
            var targetWorldPos = BoardUtils.GetWorldPosByCellPos(previousPart.Read<PositionOnBoard>().value);

            entity.SetPosition(worldPos);

            entity.Set<IsSnakePart>();
            entity.Set(new SnakeLink { prevPart = previousPart });
            entity.Set(new PositionOnBoard { value = boardCellPos });
            entity.Set(new StartMovePosition { value = worldPos });
            entity.Set(new PrevPositionInfo
            {
                position = worldPos,
                direction = prevPartDirection
            });
            entity.Set(new TargetPosition { value = targetWorldPos });

            entity.InstantiateView(snakePartViewId);

            return entity;
        }

        private Entity CreateEntityWithConfig(ConfigBase config)
        {
            var entity = world.AddEntity("Snake");
            config.Apply(entity);
            return entity;
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
            return Filter.Create("Filter-SnakeSpawnSystem")
                .With<SpawnSnakePartEvent>()
                .WithShared<GameInfo>()
                .WithoutShared<GamePaused>()
                .Push();
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            lastPartOfSnake = lastPartOfSnake.IsEmpty()
                ? CreateSnakeHead(snakeHeadConfig, entity.Read<SnakeStartPosition>().startPosition)
                : CreateSnakePart(snakePartConfig, lastPartOfSnake, entity.Has<IsInitPart>());

            world.RemoveEntity(entity);

            ref var gameInfo = ref world.GetSharedData<GameInfo>();
            gameInfo.snakeLength++;
            world.AddMarker(new UpdateProgressMarker());
        }
    }
}