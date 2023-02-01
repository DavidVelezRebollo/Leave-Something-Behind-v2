using LSB.Interfaces;
using LSB.Input;
using UnityEngine;

namespace LSB.Classes.Player {
    public class PlayerMovement {
        private readonly Rigidbody2D _rigidbody2D;
        private readonly InputHandler _input;
        
        public PlayerMovement(Rigidbody2D rigidbody2D) {
            _rigidbody2D = rigidbody2D;
            
            
            _input = InputHandler.Instance;
        }
        
        public void Move(float speed) {
            Vector2 movement = new Vector2(_input.GetMovement().x, _input.GetMovement().y);
            _rigidbody2D.MovePosition(_rigidbody2D.position + speed * Time.fixedDeltaTime * movement.normalized);
        }
    }
}
