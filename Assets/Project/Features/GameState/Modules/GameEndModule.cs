using ME.ECS;
using Project.Components;
using Project.Markers;

namespace Project.Features.GameState.Modules {
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class GameEndModule : IModule, IUpdate {
        
        private GameStateFeature feature;
        
        public World world { get; set; }
        
        void IModuleBase.OnConstruct() {
            
            this.feature = this.world.GetFeature<GameStateFeature>();
            
        }
        
        void IModuleBase.OnDeconstruct() {}

        void IUpdate.Update(in float deltaTime)
        {
            if (world.GetMarker(out GameEndedMarker gameEndedMarker))
            {
                feature.endedGameDataReceived.Execute();
                world.SetSharedData(new GameEndInfo()
                {
                    playedTime = gameEndedMarker.playedTime,
                    gameInfo = new GameInfo()
                    {
                        snakeLength = gameEndedMarker.snakeLength,
                        appleCount = gameEndedMarker.collectedApples
                    },
                });
                world.RemoveMarker<GameEndedMarker>();
            }
        }
    }
}
