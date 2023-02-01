using UnityEngine;

namespace LSB.Shared {
    [CreateAssetMenu(fileName = "Stat", menuName = "Game/Stats")]
    public class Stats : ScriptableObject {
        public float Damage;
        public float MaxHp;
        public float Speed;
        public float AttackCooldown;
    }
}
