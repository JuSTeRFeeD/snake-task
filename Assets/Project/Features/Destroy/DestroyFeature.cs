using ME.ECS;

namespace Project.Features.Destroy
{
    using Components;
    using Modules;
    using Systems;
    using Features;
    using Markers;
    using DestroyOverTime.Components;
    using DestroyOverTime.Modules;
    using DestroyOverTime.Systems;
    using DestroyOverTime.Markers;

    namespace DestroyOverTime.Components
    {
    }

    namespace DestroyOverTime.Modules
    {
    }

    namespace DestroyOverTime.Systems
    {
    }

    namespace DestroyOverTime.Markers
    {
    }

#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class DestroyFeature : Feature
    {
        protected override void OnConstruct()
        {
            AddSystem<DespawnSystem>();
            AddSystem<DestroyOverTimeSystem>();
        }

        protected override void OnDeconstruct()
        {
        }
    }
}