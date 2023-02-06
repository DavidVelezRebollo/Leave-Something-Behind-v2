using LSB.Shared;
using UnityEngine;

namespace LSB.Components.Items {
    public class Cigarettes : Item {
        [SerializeField] private Stats PlayerStats;
        private const float _LIVE_PERCENTAGE = 0.95f;
        private const float _ATTACK_SPEED_PERCENTAGE = 1.15f;

        public override void UseItem() {
            PlayerStats.MaxHp *= _LIVE_PERCENTAGE;
            PlayerStats.AttackCooldown /= 1.15f;
        }

        public override void UndoItem() {
            PlayerStats.MaxHp /= _LIVE_PERCENTAGE;
            PlayerStats.AttackCooldown *= 1.15f;
        }
    }
}
