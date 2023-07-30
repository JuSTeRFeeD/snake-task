using ME.ECS;
using Project.Components;
using Project.Features.Initialization.Modules;

namespace Project.Features.Initialization
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class InitializationFeature : Feature
    {
        protected override void OnConstruct()
        {
            AddModule<StartGameModule>();

            world.SetSharedData(new GamePaused());
        }

        protected override void OnDeconstruct()
        {
        }
    }
}