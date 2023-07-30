using System.Collections.Generic;
using ME.ECS;
using Project.Features.Destroy.Components;
using Project.Features.ParticleEffects.Components;

namespace Project.Features.ParticleEffects.Systems
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class DespawnParticleSystem : ISystemFilter
    {
        private ParticleEffectsFeature feature;

        private readonly Dictionary<int, ViewId> effects = new();

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
            return Filter.Create("Filter-DespawnParticleSystem")
                .With<OnDespawnParticle>()
                .With<ToDespawn>()
                .Push();
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            ref readonly var onDespawnParticle = ref entity.Read<OnDespawnParticle>();
            var particle = world.AddEntity("fvx");
            particle.Set(new TimeToDestroy() { value = onDespawnParticle.timeToDespawn });
            particle.SetPosition(entity.GetPosition());

            var instId = onDespawnParticle.view.GetInstanceID();
            if (!effects.ContainsKey(instId))
            {
                var viewId = world.RegisterViewSource(onDespawnParticle.view);
                effects.Add(instId, viewId);
            }
            particle.InstantiateView(effects[instId]);
        }
    }
}