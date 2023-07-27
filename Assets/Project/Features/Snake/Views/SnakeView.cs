using ME.ECS;
using Unity.Mathematics;
using UnityEngine;

namespace Project.Features.Snake.Views
{
    using ME.ECS.Views.Providers;

    public class SnakeView : MonoBehaviourView
    {
        public override bool applyStateJob => true;

        public override void OnInitialize()
        {
        }

        public override void OnDeInitialize()
        {
        }

        public override void ApplyStateJob(UnityEngine.Jobs.TransformAccess t, float deltaTime, bool immediately)
        {
        }

        public override void ApplyState(float deltaTime, bool immediately)
        {
            transform.position = entity.GetPosition();
        }
    }
}