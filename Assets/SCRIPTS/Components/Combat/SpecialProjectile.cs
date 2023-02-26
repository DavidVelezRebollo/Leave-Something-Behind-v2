using System;
using System.Collections;
using LSB.Components.Audio;
using LSB.Components.Core;
using UnityEngine;

namespace LSB.Components.Combat {
    public class SpecialProjectile : ProjectileComponent {
        [Tooltip("Area affected by the projectile explosion")]
        [SerializeField] private GameObject ExplosionArea;
        [Tooltip("Special Projectile explosion prefab")]
        [SerializeField] private GameObject ExplosionPrefab;
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
            if (GameManager.GamePaused()) {
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
            GameObject explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            SoundManager.Instance.Play("Explosion");
            Destroy(explosion, 1f);
            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
        }
    }
}
