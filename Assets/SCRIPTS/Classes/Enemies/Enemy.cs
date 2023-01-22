using System;
using LSB.Components.Combat;
using LSB.Interfaces;
using UnityEngine;
using System.Collections;
using LSB.Components.Core;
using Object = UnityEngine.Object;

namespace LSB.Classes.Enemies {
    public class Enemy {
        private readonly IEnemyMove _movement;
        private readonly IAttack _attack;
        private IState _currentState;

        private readonly GameManager _gameManager;
        public Action<GameObject> OnEnemyDie;

        private readonly SpriteRenderer _renderer;
        private Transform _player;
        private float _currentHp;

        public Enemy(IEnemyMove movementType, IAttack attackType, SpriteRenderer renderer, float currentHp) {
            _movement = movementType;
            _attack = attackType;
            _renderer = renderer;
            _currentHp = currentHp;
            
            _gameManager = GameManager.Instance;
        }
        
        public void Start() {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public void TakeDamage(float amount) {
            _currentHp -= amount;
        }

        public void Move() {
            if (_gameManager.GameEnded() || _gameManager.GamePaused()) return;
            _movement?.Move(_player.position);
        }

        public void Attack() {
            if (_gameManager.GameEnded() || _gameManager.GamePaused()) return;
            _attack?.Attack();
        }

        public IState GetCurrentState() {
            return _currentState;
        }
        
        public void SetState(IState state) {
            _currentState?.Exit();

            _currentState = state;
            _currentState.Enter();
        }

        public bool OnCollide(Collision2D col, GameObject gameObject) {
            if (!col.collider.CompareTag("Bullets")) return false;

            TakeDamage(col.collider.GetComponent<ProjectileComponent>().GetDamage());

            if (_currentHp <= 0) {
                Die(gameObject);
            }

            return true;
        }

        private void Die(GameObject enemy) {
            OnEnemyDie?.Invoke(enemy);
            Object.Destroy(enemy);
        }

        public IEnumerator ChangeColor(Color color) {
            _renderer.color = color;
            yield return new WaitForSeconds(0.1f);
            _renderer.color = Color.white;
        }
    }
}
