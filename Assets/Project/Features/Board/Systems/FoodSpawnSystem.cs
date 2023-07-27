﻿using ME.ECS;
using ME.ECS.DataConfigs;
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
    public sealed class FoodSpawnSystem : ISystemFilter
    {
        private BoardFeature feature;

        private ViewId bananaViewId;
        private ViewId appleViewId;

        private DataConfig bananaConfig;
        private DataConfig appleConfig;
        
        public World world { get; set; }

        void ISystemBase.OnConstruct()
        {
            this.GetFeature(out this.feature);

            appleConfig = Resources.Load<DataConfig>("Food/AppleConfig");
            bananaConfig = Resources.Load<DataConfig>("Food/BananaConfig");
            
            RegisterViews();
        }

        private void RegisterViews()
        {
            var appleView = appleConfig.Get<DataConfigViewReference>().prefabView;
            appleViewId = world.RegisterViewSource(appleView);
            var bananaView = bananaConfig.Get<DataConfigViewReference>().prefabView;
            bananaViewId = world.RegisterViewSource(bananaView);
        }

        private void SpawnApple()
        {
            var entity = world.AddEntity("Apple");
            appleConfig.Apply(entity);
            entity.Get<PositionOnBoard>();
            entity.Get<ChangePositionEvent>();
            entity.InstantiateView(appleViewId);

            entity.SetPosition(feature.GetRandomEmptyBoardPosition());
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
            return Filter.Create("Filter-Food-Spawn-System")
                .With<SpawnApple>()
                .Push();
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            SpawnApple();
            world.RemoveEntity(entity);
        }
    }
}