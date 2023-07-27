﻿using ME.ECS;
using Project.Utilities;
using UnityEngine;

namespace Project.Features.Board.Systems
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
    public sealed class BoardCellSystem : ISystemFilter, IDrawGizmos
    {
        private BoardFeature feature;

#if UNITY_EDITOR
        private Filter cellsFilter;
#endif

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
            return Filter.Create("Filter-BoardCellSystem")
                .With<PositionOnBoard>()
                .With<ChangePositionEvent>()
                .Push();
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            var cellPos = BoardUtils.GetCellPos(entity.GetPosition()); 
            feature.UpdateBoardEntity(entity, cellPos);
            entity.Remove<ChangePositionEvent>();
        }

        public void OnDrawGizmos()
        {
            var size = new Vector3(BoardUtils.GridCellSize - 0.1f, 0, BoardUtils.GridCellSize - 0.1f);

            foreach (var i in feature.cells)
            {
                if (!i.Value.entity.IsEmpty())
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(i.Value.worldPosition, size);
                }
                else
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(i.Value.worldPosition, size);
                }
            }
        }
    }
}