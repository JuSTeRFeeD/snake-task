using ME.ECS;
using Project.Features.Destroy.Components;
using TMPro;
using UnityEngine;

namespace Project.Features.Board.Views {
    
    using ME.ECS.Views.Providers;
    
    public class FoodView : MonoBehaviourView
    {
        [SerializeField] private Canvas timerCanvas;
        [SerializeField] private TextMeshProUGUI timerText;
        
        public override bool applyStateJob => true;

        public override void OnInitialize() {
            if (timerCanvas)
            {
                timerCanvas.gameObject.SetActive(entity.Has<TimeToDestroy>());
            }
        }
        
        public override void OnDeInitialize() {
            
        }
        
        public override void ApplyStateJob(UnityEngine.Jobs.TransformAccess transform, float deltaTime, bool immediately) {
            
        }
        
        public override void ApplyState(float deltaTime, bool immediately)
        {
            transform.position = entity.GetPosition();
            
            if (entity.Has<TimeToDestroy>())
            {
                var time = entity.Get<TimeToDestroy>().value;
                timerText.SetText($"{(int)time}");
            }
        }
        
    }
}