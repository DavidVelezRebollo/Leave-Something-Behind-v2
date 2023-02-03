using UnityEngine;
using LSB.Shared;

namespace LSB.Components.Items {
    public class Scooter : Item {
        [SerializeField] private Stats PlayerStats;
        private const float _SPEED_PERCENTAGE = 1.15f;

        public override void UseItem() { PlayerStats.Speed *= _SPEED_PERCENTAGE; }

        public override void UndoItem() { PlayerStats.Speed /= _SPEED_PERCENTAGE; }
    }
}

