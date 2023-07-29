using ME.ECS;
using Project.Markers;
using Unity.Mathematics;

namespace Project.Features.Input.Modules
{
    using Components;
    using Modules;
    using Systems;
    using Features;
    using Markers;

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
            var x = UnityEngine.Input.GetAxisRaw("Horizontal");
            var y = UnityEngine.Input.GetAxisRaw("Vertical");

            if (x > 0) x = 1;
            else if (x < 0) x = -1;
            else if (y > 0) y = 1;
            else if (y < 0) y = -1;

            if (x != 0 && y != 0) y = 0;
            
            var input = new int2((int)x, (int)y);
            
            if (input.Equals(int2.zero)) return;
                
            world.AddMarker(new PlayerMoveInputMarker()
            {
                value = input
            });
        }
    }
}