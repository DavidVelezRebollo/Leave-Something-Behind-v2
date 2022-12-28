using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace LSB.Components.Items {
    public class Glasses : Item {
        private PostProcessVolume _postProcessVolume;

        public override void UseItem() {
            if (Camera.main == null) {
                Debug.LogWarning("Main Camera null exception");
                return;
            }
                
            _postProcessVolume = Camera.main.GetComponent<PostProcessVolume>();
        }

        public override void UndoItem()
        {
            _postProcessVolume.weight = 1;
        }
    }
}

