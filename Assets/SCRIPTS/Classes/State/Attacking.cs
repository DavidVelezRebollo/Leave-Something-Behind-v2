using LSB.Classes.Enemies;
using LSB.Interfaces;
using UnityEngine;

namespace LSB.Classes.State {
    public class Attacking : IState {
        private readonly Enemy _enemy;
        private readonly float _stopDistance;
        private readonly Transform _enemyTransform;
        private Transform _player;
        private readonly Rigidbody2D _rigidBody;

        public Attacking(Enemy enemy, Transform transform, float stopDistance, Rigidbody2D rb) {
            _enemy = enemy;
            _stopDistance = stopDistance;
            _enemyTransform = transform;
            _rigidBody = rb;
        }
        
        public void Enter() {
            _player = GameObject.FindWithTag("Player").transform;
        }

        public void Exit() { }

        public void FixedUpdate() {
            float distance = Vector3.Distance(_player.position, _enemyTransform.position);

            if (distance <= _stopDistance)
            {
                _rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
                _enemy.Attack();
            }
            else
                _enemy.SetCurrentState(new Chasing(_enemy, _enemyTransform, _stopDistance, _rigidBody));
        }
    }
}
