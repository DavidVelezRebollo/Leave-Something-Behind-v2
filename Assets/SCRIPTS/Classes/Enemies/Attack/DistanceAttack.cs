using System;
using LSB.Components.Combat;
using LSB.Interfaces;
using UnityEngine;

namespace LSB.Classes.Enemies {
    public class DistanceAttack : MonoBehaviour, IAttack {
        [SerializeField] private GameObject ProjectilePrefab;
        [SerializeField] private float AttackCooldown;

        private Transform _playerTransform;
        private float _cooldownDelta;

        private void OnEnable() {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            ProjectilePrefab.GetComponent<ProjectileComponent>().Reset();
        }

        public void Attack() {
            _cooldownDelta -= Time.deltaTime;
            if (_cooldownDelta > 0) return;
            
            Vector3 position = transform.position;

            Vector2 dir = _playerTransform.position - position;
            Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x)  * Mathf.Rad2Deg);

            GameObject projectile = Instantiate(ProjectilePrefab, position, rotation);
            
            float speed = projectile.GetComponent<ProjectileComponent>().GetSpeed();
            projectile.GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;

            _cooldownDelta = AttackCooldown;
        }
    }
}

