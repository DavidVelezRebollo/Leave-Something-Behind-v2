using UnityEngine;

namespace LSB.Components.Items {
    public class Potion : MonoBehaviour {
        [Tooltip("Amount of life recovered when picked up")]
        [SerializeField] private float RecoveryAmount;
        [Tooltip("Time between the potion dispairs")]
        [SerializeField] private float DestroyTime;

        private float _destroyDelta;
        

        private void Start() {
            _destroyDelta = DestroyTime;
        }

        private void Update() {
            _destroyDelta -= Time.deltaTime;

            if (_destroyDelta <= 0)
                Destroy(gameObject);
        }

        public float GetRecoveryAmount() { return RecoveryAmount; }
    }
}
