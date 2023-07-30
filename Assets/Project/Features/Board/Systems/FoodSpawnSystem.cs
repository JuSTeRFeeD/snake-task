using ME.ECS;
using ME.ECS.DataConfigs;
using Project.Components;
using Project.Features.Board.Components;
using Project.Features.ParticleEffects.Components;
using UnityEngine;

namespace Project.Features.Board.Systems
{
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

        private const int SpawnBananaRate = 5;

        public ViewId eatEffectViewId;

        public World world { get; set; }

        void ISystemBase.OnConstruct()
        {
            this.GetFeature(out feature);

            appleConfig = Resources.Load<DataConfig>("Food/AppleConfig");
            bananaConfig = Resources.Load<DataConfig>("Food/BananaConfig");

            RegisterViews();
        }

        private void RegisterViews()
        {
            var appleView = appleConfig.Read<DataConfigViewReference>().prefabView;
            appleViewId = world.RegisterViewSource(appleView);
            var bananaView = bananaConfig.Read<DataConfigViewReference>().prefabView;
            bananaViewId = world.RegisterViewSource(bananaView);
        }

        private void SpawnFood(ConfigBase config, ViewId viewId)
        {
            if (!feature.GetRandomEmptyBoardPosition(out var position)) return;
            var entity = world.AddEntity("Food");
            config.Apply(entity);
            entity.Set<PositionOnBoard>();
            entity.Set<ChangePositionEvent>();
            entity.SetPosition(position);
            entity.InstantiateView(viewId);
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
                .With<SpawnFoodEvent>()
                .WithoutShared<GamePaused>()
                .Push();
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            if (!entity.Read<SpawnFoodEvent>().isInitSpawn &&
                world.ReadSharedData<GameInfo>().appleCount % SpawnBananaRate == 0)
            {
                SpawnFood(bananaConfig, bananaViewId);
            }
            else
            {
                SpawnFood(appleConfig, appleViewId);
            }

            world.RemoveEntity(entity);
        }
    }
}