using System;
using System.Collections;
using LSB.Components.Audio;
using LSB.Components.Core;
using UnityEngine;

namespace LSB.Components.Combat {
    public class SpecialProjectile : ProjectileComponent {
        [SerializeField] private GameObject ExplosionArea;
        private Vector3 _targetPosition;
        private SpriteRenderer _projectileRenderer;

        private void OnEnable() {
            _projectileRenderer = GetComponentInParent<SpriteRenderer>();
            ExplosionArea.SetActive(false);
        }

        public void SetTargetPosition(Vector3 target) {
            _targetPosition = target;
        }

        protected override void Update() {
            if (GameManager.GamePaused() || GameManager.GameEnded()) {
                Rb.velocity = Vector2.zero;
                return;
            }

            if (Vector3.Distance(transform.position, _targetPosition) <= 0.2f) {
                StartCoroutine(explode());
                return;
            }

            Rb.velocity = Velocity;
        }

        private IEnumerator explode() {
            ExplosionArea.SetActive(true);
            Rb.velocity = Vector2.zero;
            _projectileRenderer.sprite = null;
            SoundManager.Instance.Play("Explosion");
            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
        }
    }
}
