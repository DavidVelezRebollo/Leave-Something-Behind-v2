using LSB.Interfaces;
using UnityEngine;

namespace LSB.Classes.Enemies {
    public class Chase : IEnemyMove {
        private Rigidbody2D _rigidbody2D;
        private Transform _transform;
        private float _speed;

        public Chase(Transform transform, Rigidbody2D rb, float speed) {
            _rigidbody2D = rb;
            _transform = transform;
            _speed = speed;
        }
        
        public void Move(Vector3 playerPosition) {
            Vector2 dir = (playerPosition - _transform.position).normalized;
            _rigidbody2D.MovePosition(_rigidbody2D.position + _speed * Time.fixedDeltaTime * dir);
        }
    }
}