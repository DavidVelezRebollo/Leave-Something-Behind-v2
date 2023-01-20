using LSB.Interfaces;
using LSB.Input;
using UnityEngine;

namespace LSB.Classes.Player {
    public class PlayerMovement {
        private readonly Rigidbody2D _rigidbody2D;
        private readonly float _speed;
        private readonly InputHandler _input;
        
        public PlayerMovement(Rigidbody2D rigidbody2D, float speed) {
            _rigidbody2D = rigidbody2D;
            _speed = speed;
            
            _input = InputHandler.Instance;
        }
        
        public void Move() {
            Vector2 movement = new Vector2(_input.GetMovement().x, _input.GetMovement().y);
            _rigidbody2D.MovePosition(_rigidbody2D.position + _speed * Time.fixedDeltaTime * movement.normalized);
        }
    }
}
