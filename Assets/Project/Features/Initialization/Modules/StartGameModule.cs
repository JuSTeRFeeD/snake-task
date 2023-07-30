using ME.ECS;
using Project.Features.GameState;
using Project.Markers;

namespace Project.Features.Initialization.Modules
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class StartGameModule : IModule, IUpdate
    {
        private GameStateFeature gameStateFeature;

        public World world { get; set; }

        void IModuleBase.OnConstruct()
        {
            gameStateFeature = world.GetFeature<GameStateFeature>();
        }

        void IModuleBase.OnDeconstruct()
        {
        }

        void IUpdate.Update(in float deltaTime)
        {
            if (world.GetMarker(out GameCreatedMarker gameCreatedMarker))
            {
                gameStateFeature.StartGame(gameCreatedMarker);
            }
        }
    }
}