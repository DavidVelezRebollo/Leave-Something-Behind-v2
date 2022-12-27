using LSB.Components.Combat;
using LSB.Interfaces;
using LSB.Shared;
using UnityEngine;
using System.Collections;

namespace LSB.Classes.Enemies {
    public class Enemy
    {
        private readonly IEnemyMove _movement;
        private IAttack _attack;
        private IState _state;
        private Stats _attributes;

        private readonly SpriteRenderer _renderer;
        private Transform _player;
        private float _currentHp;

        public Enemy(IEnemyMove movementType, IAttack attackType, Stats attributes, SpriteRenderer renderer) {
            _movement = movementType;
            _attack = attackType;
            _attributes = attributes;
            _renderer = renderer;

            _currentHp = attributes.MaxHp;
        }
        
        public void Start() {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public void TakeDamage(float amount) {
            _currentHp -= amount;
        }

        public void Move() {
            _movement?.Move(_player.position);
        }

        public void OnCollide(Collision2D col, GameObject gameObject) {
            if (!col.collider.CompareTag("Bullets")) return;

            TakeDamage(col.collider.GetComponent<Arrow>().GetDamage());

            if (_currentHp > 0) return;

            Die(gameObject);
        }

        public void Die(GameObject enemy) {
            Object.Destroy(enemy);
        }

        public IEnumerator ChangeColor(Color color) {
            _renderer.color = color;
            yield return new WaitForSeconds(0.1f);
            _renderer.color = Color.white;
        }
    }
}
