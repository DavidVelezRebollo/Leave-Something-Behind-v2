using LSB.Interfaces;
using UnityEngine;

namespace LSB.Classes.Enemies {
    public class Chase : IEnemyMove {
        private readonly Rigidbody2D _rigidbody2D;
        private readonly Transform _transform;
        private readonly float _speed;
        private Vector2 _lastDirection;
        //private GameObject _enemy;
        //private Enemy _die;

        public Chase(Transform transform, Rigidbody2D rb, float speed) {
            _rigidbody2D = rb;
            _transform = transform;
            _speed = speed;
        }
        
        public void Move(Vector3 playerPosition) {
            float distance = Vector3.Distance(playerPosition, _transform.position);
            //if(distance > 5) { _die.Die(_enemy);  return; }

            Vector2 dir = (playerPosition - _transform.position).normalized;
            _lastDirection = dir;
            _rigidbody2D.MovePosition(_rigidbody2D.position + _speed * Time.fixedDeltaTime * dir);
        }

        public Vector2 GetLastDirection()
        {
            return _lastDirection;
        }
    }
}