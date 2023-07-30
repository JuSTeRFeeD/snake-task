using ME.ECS;
using Project.Components;
using Project.Features.GameState.Modules;
using Project.Markers;

namespace Project.Features.GameState
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class GameStateFeature : Feature
    {
        public GlobalEvent startGame;
        public GlobalEvent endGame;
        public GlobalEvent endedGameDataReceived;

        protected override void OnConstruct()
        {
            AddModule<GameEndModule>();
        }

        protected override void OnDeconstruct() { }

        public void EndGame()
        {
            world.SetSharedData(new GamePaused());
            world.AddMarker(new GameEndMarker());
            endGame.Execute();
        }
        
        public void StartGame(GameCreatedMarker gameCreatedMarker)
        {
            world.SetSharedData(new GameInfo()
            {
                gameId = gameCreatedMarker.gameId,
                appleCount = 0
            });
            world.RemoveMarker<GameCreatedMarker>();
            world.RemoveSharedData<GamePaused>();
            startGame.Execute();
        }
    }
}