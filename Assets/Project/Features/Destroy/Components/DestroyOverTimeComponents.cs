using ME.ECS;

namespace Project.Features.DestroyOverTime.Components {

    public struct TimeToDestroy : IComponent
    {
        public float value;
    }

    public struct ToDespawn : IComponent
    {
    }
}