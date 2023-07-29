using ME.ECS;

namespace Project.Features.Destroy.Components {

    public struct TimeToDestroy : IComponent
    {
        public float value;
    }

    public struct ToDespawn : IComponent
    {
    }
}