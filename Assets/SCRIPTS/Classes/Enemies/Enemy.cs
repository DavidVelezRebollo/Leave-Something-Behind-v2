using System.Collections;
using LSB.Interfaces;
using LSB.Shared;
using UnityEngine;

namespace LSB.Classes.Enemies {
    public class Enemy
    {
        private readonly IEnemyMove _movement;
        private IAttack _attack;
        private IState _state;
        private Stats _attributes;

        private Transform _player;
        private SpriteRenderer _renderer;
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

        public bool TakeDamage(float amount) {
            if (_currentHp - amount <= 0) return true;

            _currentHp -= amount;
            Debug.Log(_currentHp);
            
            return false;
        }

        public void Move() {
            _movement?.Move(_player.position);
        }

        public void Die(GameObject enemy) {
            GameObject.Destroy(enemy);
        }

        public IEnumerator ChangeColor(Color color) {
            _renderer.color = color;
            yield return new WaitForSeconds(0.1f);
            _renderer.color = Color.white;
        }
    }
}
