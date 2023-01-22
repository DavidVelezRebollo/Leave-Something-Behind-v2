using LSB.Classes.Enemies;
using LSB.Interfaces;
using UnityEngine;

namespace LSB.Classes.State {
    public class Chasing : IState {
        private readonly Enemy _enemy;
        private readonly float _stopDistance;
        private readonly Transform _enemyTransform;
        private Transform _player;
        private Rigidbody2D _rigidBody;
        
        public Chasing(Enemy enemy, Transform transform, float stopDistance, Rigidbody2D rb) {
            _enemy = enemy;
            _stopDistance = stopDistance;
            _enemyTransform = transform;
            _rigidBody = rb;
        }

        public void Enter() {
            _player = GameObject.FindWithTag("Player").transform;
        }

        public void Exit() {
        }

        public void FixedUpdate() {
            float distance = Vector3.Distance(_player.position, _enemyTransform.position);

            if (distance > _stopDistance)
            {
                _rigidBody.constraints = RigidbodyConstraints2D.None;
                _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                _enemy.Move();
            }
            else
                _enemy.SetState(new Attacking(_enemy, _enemyTransform, _stopDistance, _rigidBody));
        }
    }
    
}
