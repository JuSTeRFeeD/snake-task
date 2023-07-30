using ME.ECS;
using Project.Features.ParticleEffects.Views;

namespace Project.Features.ParticleEffects.Components {

    public struct OnDespawnParticle : IComponent
    {
        public ParticleEffectView view;
        public int timeToDespawn;
    }
}