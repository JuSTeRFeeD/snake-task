using System.Collections.Generic;
using System.Linq;
using ME.ECS;
using ME.ECS.DataConfigs;
using Project.Components;
using Project.Utilities;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Project.Features.Board
{
    using Components;
    using Modules;
    using Systems;
    using Features;
    using Markers;
    using Board.Components;
    using Board.Modules;
    using Board.Systems;
    using Board.Markers;

    namespace Board.Components
    {
    }

    namespace Board.Modules
    {
    }

    namespace Board.Systems
    {
    }

    namespace Board.Markers
    {
    }

#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class BoardFeature : Feature
    {
        private BoardSize boardSize;

        public readonly Dictionary<int2, BoardCell> cells = new ();

        protected override void OnConstruct()
        {
            AddSystem<BoardCellSystem>();
            AddSystem<FoodSpawnSystem>();

            InitializeBoardCells();

            var group = world.AddEntities(100, Allocator.Temp, true);
            group.Set(new SpawnApple());
        }

        private void InitializeBoardCells()
        {
            var boardConfig = Resources.Load<DataConfig>("BoardConfig");
            boardSize = boardConfig.Get<BoardSize>();

            for (var x = 0; x < boardSize.sizeX; x++)
            {
                for (var y = 0; y < boardSize.sizeY; y++)
                {
                    var cellPos = new int2(x, y);
                    var worldPos = BoardUtils.GetWorldPosByCellPos(cellPos);
                    cells.Add(cellPos, new BoardCell()
                    {
                        worldPosition = worldPos
                    });
                }
            }
        }

        protected override void OnDeconstruct()
        {
        }

        public void UpdateBoardEntity(Entity entity, int2 cellPos)
        {
            if (cellPos.x < 0 || cellPos.y < 0 || cellPos.x > boardSize.sizeX || cellPos.y > boardSize.sizeY) return;

            ref var positionOnBoard = ref entity.Get<PositionOnBoard>();
            var prevCell = cells[positionOnBoard.value];
            if (prevCell.entity.Equals(entity))
            {
                prevCell.entity = default;
            }
            
            var cell = cells[cellPos];
            if (!cell.entity.IsEmpty())
            {
                cell.entity.Get<CollisionWithEntity>().entity = entity;
                entity.Get<CollisionWithEntity>().entity = cell.entity;
            }
            cell.entity = entity;
            positionOnBoard.value = cellPos;
        }

        public float3 GetRandomEmptyBoardPosition()
        {
            var emptyCount = 0;
            float3 emptyPosition = default;

            foreach (var cell in cells
                         .Select(kvp => kvp.Value)
                         .Where(cell => cell.entity.IsEmpty())
                     )
            {
                emptyCount++;
                if (world.GetRandomRange(0, emptyCount) == 0)
                {
                    emptyPosition = cell.worldPosition;
                }
            }

            return emptyPosition;
        }
    }
}