using ME.ECS;
using Project.Features.ParticleEffects.Systems;

namespace Project.Features.ParticleEffects
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class ParticleEffectsFeature : Feature
    {
        protected override void OnConstruct()
        {
            AddSystem<DespawnParticleSystem>();
        }

        protected override void OnDeconstruct()
        {
        }
    }
}