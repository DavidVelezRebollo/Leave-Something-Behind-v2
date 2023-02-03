using UnityEngine;
using LSB.Shared;

namespace LSB.Components.Items {
    public class ConstructionHelmet : Item {
        [SerializeField] private Stats OrcStats;
        private const float _DAMAGE_PERCENTAGE = 0.9f;

        public override void UseItem() { OrcStats.Damage *= _DAMAGE_PERCENTAGE; }

        public override void UndoItem() { OrcStats.Damage /= _DAMAGE_PERCENTAGE; }
    }
}