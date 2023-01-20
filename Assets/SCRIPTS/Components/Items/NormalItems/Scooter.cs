using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using LSB.Components.Audio;
using LSB.Shared;

namespace LSB.Components.Items {
    public class Scooter : Item {
        [SerializeField] private Stats PlayerStats;
        private const float _SPEED_PERCENTAGE = 1 + 0.15f;

        public override void UseItem() {
            PlayerStats.Speed *= _SPEED_PERCENTAGE;
        }

        public override void UndoItem()
        {
            PlayerStats.Speed /= _SPEED_PERCENTAGE;
        }
    }
}

