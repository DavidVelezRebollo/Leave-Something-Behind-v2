using UnityEngine;
using LSB.Shared;

namespace LSB.Components.Items {
    public class Catbucks : Item {
        [SerializeField] private Stats PlayerStats;
        private const float _LIVE_PERCENTAGE = 1.1f;

        public override void UseItem() { PlayerStats.MaxHp *= _LIVE_PERCENTAGE; }

        public override void UndoItem() { PlayerStats.MaxHp /= _LIVE_PERCENTAGE; }
    }
}