﻿using ME.ECS;
using ME.ECS.DataConfigs;
using Project.Features.Board;
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
    public sealed class SnakeSpawnSystem : ISystemFilter
    {
        private SnakeFeature feature;
        private BoardFeature boardFeature;

        public World world { get; set; }

        private ViewId snakeHeadViewId;
        private ViewId snakePartViewId;

        private DataConfig snakePartConfig;
        private DataConfig snakeHeadConfig;
        
        private Entity lastPartOfSnake;

        void ISystemBase.OnConstruct()
        {
            this.GetFeature(out feature);
            this.GetFeature(out boardFeature);

            snakeHeadConfig = Resources.Load<DataConfig>("SnakeHeadConfig");
            snakePartConfig = Resources.Load<DataConfig>("SnakePartConfig");
            
            RegisterViews();
        }

        private void RegisterViews()
        {
            var headView = snakeHeadConfig.Get<DataConfigViewReference>().prefabView;
            snakeHeadViewId = world.RegisterViewSource(headView);
            var partView = snakePartConfig.Get<DataConfigViewReference>().prefabView;
            snakePartViewId = world.RegisterViewSource(partView);
        }

        private Entity CreateSnakeHead(ConfigBase config, int2 boardCellPos)
        {
            var entity = CreateEntityWithConfig(config);

            var worldPos = BoardUtils.GetWorldPosByCellPos(boardCellPos);

            var initMoveDir = new int2(0, 1);
            
            entity.SetPosition(worldPos);

            entity.Get<IsSnakeHead>();
            entity.Get<IsSnakePart>();
            entity.Get<PositionOnBoard>().value = boardCellPos;
            entity.Get<StartMovePosition>().value = worldPos;
            entity.Get<PrevPositionInfo>().direction = initMoveDir;
            entity.Get<PrevPositionInfo>().position = BoardUtils.GetWorldPosByCellPos(boardCellPos - initMoveDir);
            entity.Get<TargetPosition>().value = BoardUtils.GetWorldPosByCellPos(boardCellPos + initMoveDir);
            entity.Get<MoveDirection>().value = initMoveDir;
            
            entity.InstantiateView(snakeHeadViewId);
            
            // var entity = CreateEntityWithConfig(config);
            //
            // ref readonly var dir = ref entity.Get<MoveDirection>().value;
            // ref var prevPositionInfo = ref entity.Get<PrevPositionInfo>();
            //
            // var worldPos = BoardUtils.GetWorldPosByCellPos(boardCellPos); 
            // var prevWorldPos = BoardUtils.GetWorldPosByCellPos(boardCellPos - new int2(0, 1));
            // var targetWorldPos = BoardUtils.GetWorldPosByCellPos(boardCellPos + new int2(0, 1));
            //
            // entity.SetPosition(worldPos);
            // entity.Get<IsSnakeHead>();
            // entity.Get<IsSnakePart>();
            // entity.Get<PositionOnBoard>().value = boardCellPos;
            // entity.Get<TargetPosition>().value = targetWorldPos;
            // entity.Get<StartMovePosition>().value = worldPos;
            // entity.Get<MoveDirection>().value = new float3(0, 0, 1);
            // prevPositionInfo.direction = dir;
            // prevPositionInfo.position = prevWorldPos;
            //     
            // entity.InstantiateView(snakeHeadViewId);
            //
            // boardFeature.UpdateBoardEntity(entity, BoardUtils.GetCellPos(worldPos));

            return entity;
        }

        private Entity CreateSnakePart(ConfigBase config, Entity previousPart)
        {
            var prevPartDirection = previousPart.Get<PrevPositionInfo>().direction;
            
            var entity = CreateEntityWithConfig(config);

            var boardCellPos = previousPart.Get<PositionOnBoard>().value - prevPartDirection;
            var prevBoardCellPos = BoardUtils.GetWorldPosByCellPos(boardCellPos - prevPartDirection);
            
            var worldPos = BoardUtils.GetWorldPosByCellPos(boardCellPos);
            var targetWorldPos = BoardUtils.GetWorldPosByCellPos(previousPart.Get<PositionOnBoard>().value);

            entity.SetPosition(worldPos);
            
            entity.Get<IsSnakePart>();
            entity.Get<SnakeLink>().prevPart = previousPart;
            entity.Get<PositionOnBoard>().value = boardCellPos;
            entity.Get<StartMovePosition>().value = worldPos;
            entity.Get<PrevPositionInfo>().position = prevBoardCellPos;
            entity.Get<PrevPositionInfo>().direction = prevPartDirection;
            entity.Get<TargetPosition>().value = targetWorldPos; 
            
            entity.InstantiateView(snakePartViewId);
            
            // var entity = CreateEntityWithConfig(config);
            //
            // ref readonly var prevPartPositionInfo = ref previousPart.Get<PrevPositionInfo>();
            // ref readonly var prevPartPositionOnBoard = ref previousPart.Get<PositionOnBoard>().value;
            //
            // var prevPartDirection = prevPartPositionInfo.direction;
            // var moveDirection = new int2((int)prevPartDirection.x, (int)prevPartDirection.z);
            //
            // var worldPos = BoardUtils.GetWorldPosByCellPos(prevPartPositionOnBoard - moveDirection);
            // var prevWorldPos = BoardUtils.GetWorldPosByCellPos(prevPartPositionOnBoard - moveDirection - moveDirection);
            // var posOnBoard = BoardUtils.GetCellPos(worldPos);
            //
            // ref var prevPositionInfo = ref entity.Get<PrevPositionInfo>();
            //
            // entity.SetPosition(worldPos);
            // entity.Get<IsSnakePart>();
            // entity.Get<StartMovePosition>().value = worldPos;
            // entity.Get<SnakeLink>().prevPart = previousPart;
            // entity.Get<PositionOnBoard>().value = posOnBoard;
            // prevPositionInfo.position = prevWorldPos;
            // prevPositionInfo.direction = prevPartDirection;
            //
            // entity.InstantiateView(snakePartViewId);
            //
            // boardFeature.UpdateBoardEntity(entity, posOnBoard);

            return entity;
        }

        private Entity CreateEntityWithConfig(ConfigBase config)
        {
            var entity = world.AddEntity("Snake Part");
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
                .With<SpawnSnakePart>()
                .Push();
        }

        // private float test = 5; // TODO: delete

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            // test -= deltaTime; // TODO: delete
            // if (test > 0) return; // TODO: delete
            
            lastPartOfSnake = lastPartOfSnake.IsEmpty()
                ? CreateSnakeHead(snakeHeadConfig, entity.Get<SnakeStartPosition>().startPosition) 
                : CreateSnakePart(snakePartConfig, lastPartOfSnake);
            
            world.RemoveEntity(entity);
        }
    }
}