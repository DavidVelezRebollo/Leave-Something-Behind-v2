using LSB.Interfaces;
using UnityEngine;

namespace LSB.Classes.Enemies {
    public class Chase : IEnemyMove {
        #region Private Fields
        
        // Enemy's RigidBody2D
        private readonly Rigidbody2D _rigidbody2D;
        // Enemy's Transform which contains the position and rotation of the enemy
        private readonly Transform _transform;
        // Enemy's speed
        private readonly float _speed;
        
        // Last direction where the enemy was looking
        private Vector2 _lastDirection;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new Chase movement type
        /// </summary>
        /// <param name="go">Enemy's GameObject</param>
        /// <param name="transform">Enemy's Transform component</param>
        /// <param name="rb">Enemy's RigidBody2D component</param>
        /// <param name="speed">Enemy's speed</param>
        public Chase(Transform transform, Rigidbody2D rb, float speed) {
            _rigidbody2D = rb;
            _transform = transform;
            _speed = speed;
        }
        
        #endregion

        #region Interface Methods
        
        public void Move(Vector3 playerPosition) {
            Vector2 dir = (playerPosition - _transform.position).normalized;
            _lastDirection = dir;
            _rigidbody2D.MovePosition(_rigidbody2D.position + _speed * Time.fixedDeltaTime * dir);
        }
        
        public Vector2 GetLastDirection()
        {
            return _lastDirection;
        }

        #endregion
    }
}