﻿using ME.ECS;
using UnityEngine;

namespace Project.Features.ParticleEffects.Views
{
    using ME.ECS.Views.Providers;

    public class ParticleEffectView : MonoBehaviourView
    {
        [SerializeField] private ParticleSystem particle;
        
        public override bool applyStateJob => true;

        public override void OnInitialize()
        {
        }

        public override void OnDeInitialize()
        {
        }

        public override void ApplyStateJob(UnityEngine.Jobs.TransformAccess transform, float deltaTime,
            bool immediately)
        {
        }

        public override void ApplyState(float deltaTime, bool immediately)
        {
            transform.position = entity.GetPosition();
        }
    }
}