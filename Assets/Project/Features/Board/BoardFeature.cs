﻿using System.Collections.Generic;
using System.Linq;
using ME.ECS;
using ME.ECS.DataConfigs;
using Project.Components;
using Project.Features.Board.Components;
using Project.Features.Board.Systems;
using Project.Utilities;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Project.Features.Board
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class BoardFeature : Feature
    {
        public BoardSize BoardSize { get; private set; }

        public readonly Dictionary<int2, BoardCell> cells = new ();
        
        protected override void OnConstruct()
        {
            AddSystem<BoardCellSystem>();
            AddSystem<FoodSpawnSystem>();

            InitializeBoardCells();
            
            SpawnFood();
        }

        private void SpawnFood()
        {
            var group = world.AddEntities(10, Allocator.Temp, true);
            group.Set(new SpawnFoodEvent() { isInitSpawn = true });
        }

        private void InitializeBoardCells()
        {
            var boardConfig = Resources.Load<DataConfig>("BoardConfig");
            BoardSize = boardConfig.Get<BoardSize>();

            for (var x = 0; x < BoardSize.sizeX; x++)
            {
                for (var y = 0; y < BoardSize.sizeY; y++)
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
            if (!CheckPosOnBoard(cellPos)) return;

            ref var positionOnBoard = ref entity.Get<PositionOnBoard>();
            if (CheckPosOnBoard(positionOnBoard.value))
            {
                var prevCell = cells[positionOnBoard.value];
                if (prevCell.entity.Equals(entity))
                {
                    prevCell.entity = default;
                }   
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
        
        public bool CheckPosOnBoard(int2 cellPos)
        {
            return cellPos.x >= 0 && cellPos.y >= 0 && cellPos.x < BoardSize.sizeX && cellPos.y < BoardSize.sizeY;
        }

        public bool GetRandomEmptyBoardPosition(out float3 emptyPosition)
        {
            var emptyExists = false;
            var emptyCount = 0;
            emptyPosition = default;

            foreach (var cell in cells
                         .Select(kvp => kvp.Value)
                         .Where(cell => cell.entity.IsEmpty())
                     )
            {
                emptyExists = true;
                emptyCount++;
                if (world.GetRandomRange(0, emptyCount) == 0)
                {
                    emptyPosition = cell.worldPosition;
                }
            }

            return emptyExists;
        }
    }
}