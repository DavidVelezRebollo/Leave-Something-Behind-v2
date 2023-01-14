using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using LSB.Components.Audio;

namespace LSB.Components.Items {
    public class Headphones : Item {

        public override void UseItem() {
            // NOTHING
        }

        public override void UndoItem()
        {
            SoundManager.Instance.setMusicVolume(0);
            SoundManager.Instance.setMusicActive(false);
        }
    }
}

