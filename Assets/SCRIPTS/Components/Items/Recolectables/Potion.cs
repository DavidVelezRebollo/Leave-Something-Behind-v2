using UnityEngine;

namespace LSB.Components.Items {
    public class Potion : MonoBehaviour {
        [SerializeField] private float RecoveryAmount;

        public float GetRecoveryAmount() { return RecoveryAmount; }
    }
}
