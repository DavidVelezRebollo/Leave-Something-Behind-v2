using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using LSB.Components.Audio;
using LSB.Components.Player;

namespace LSB.Components.Items {
    public class Scooter : Item {
        private float speedPercentage = 1.15f;

        public override void UseItem() {
            //PlayerManager.Instance.SetPlayerSpeed(speedPercentage);
        }

        public override void UndoItem()
        {
            PlayerManager.Instance.SetPlayerSpeed(1/speedPercentage);
        }
    }
}

