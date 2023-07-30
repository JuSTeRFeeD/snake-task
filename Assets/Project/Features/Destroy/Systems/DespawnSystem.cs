using ME.ECS;
using Project.Features.Board;
using Project.Features.Board.Components;
using Project.Features.Destroy.Components;

namespace Project.Features.Destroy.Systems
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class DespawnSystem : ISystemFilter
    {
        private DestroyFeature feature;
        private BoardFeature boardFeature;

        public World world { get; set; }

        void ISystemBase.OnConstruct()
        {
            this.GetFeature(out feature);
            this.GetFeature(out boardFeature);
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
            return Filter.Create("Filter-DespawnSystem")
                .With<ToDespawn>()
                .Push();
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            if (entity.Has<PositionOnBoard>())
            {
                boardFeature.UpdateBoardEntity(Entity.Empty, entity.Read<PositionOnBoard>().value);
                world.RemoveEntity(entity);
            }
            else
            {
                world.RemoveEntity(entity);
            }
        }
    }
}