using ME.ECS;
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

            return entity;
        }

        private Entity CreateSnakePart(ConfigBase config, Entity previousPart)
        {
            var entity = CreateEntityWithConfig(config);

            ref readonly var prevPartDirection = ref previousPart.Read<PrevPositionInfo>().direction;
            var boardCellPos = previousPart.Read<PositionOnBoard>().value - prevPartDirection;
            
            var worldPos = BoardUtils.GetWorldPosByCellPos(boardCellPos);
            var targetWorldPos = BoardUtils.GetWorldPosByCellPos(previousPart.Read<PositionOnBoard>().value);

            entity.SetPosition(worldPos);
            
            entity.Get<IsSnakePart>();
            entity.Get<SnakeLink>().prevPart = previousPart;
            entity.Get<PositionOnBoard>().value = boardCellPos;
            entity.Get<StartMovePosition>().value = worldPos;
            entity.Get<PrevPositionInfo>().position = worldPos;
            entity.Get<PrevPositionInfo>().direction = prevPartDirection;
            entity.Get<TargetPosition>().value = targetWorldPos;

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
                .WithoutShared<GamePaused>()
                .Push();
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            lastPartOfSnake = lastPartOfSnake.IsEmpty()
                ? CreateSnakeHead(snakeHeadConfig, entity.Read<SnakeStartPosition>().startPosition) 
                : CreateSnakePart(snakePartConfig, lastPartOfSnake);
            
            world.RemoveEntity(entity);
            
            ref var gameInfo = ref world.GetSharedData<GameInfo>();
            gameInfo.snakeLength++;
            world.AddMarker(new UpdateInfoMarker());
        }
    }
}