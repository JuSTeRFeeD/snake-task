using ME.ECS;
using Project.Markers;
using Unity.Mathematics;

namespace Project.Features.Input.Modules
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif

    public sealed class PlayerInputModule : IModule, IUpdate
    {
        private InputFeature feature;

        public World world { get; set; }

        void IModuleBase.OnConstruct()
        {
            feature = world.GetFeature<InputFeature>();
        }

        void IModuleBase.OnDeconstruct()
        {
        }

        void IUpdate.Update(in float deltaTime)
        {
            var input = int2.zero;
            if (world.GetMarker(out PlayerMoveInputMarker inputMarker))
            {
                input = inputMarker.value;
            }
            
            var x = UnityEngine.Input.GetAxisRaw("Horizontal");
            var y = UnityEngine.Input.GetAxisRaw("Vertical");

            if (x != 0) input = new int2((int)x, 0);
            else if (y != 0) input = new int2(0, (int)y);

            if (input.Equals(int2.zero)) return;
                
            world.AddMarker(new PlayerMoveInputMarker()
            {
                value = input
            });
        }
    }
}