using LSB.Input;
using UnityEngine;

namespace LSB.Classes.Player {
    public class PlayerMovement {
        #region Private Fields

        private readonly Rigidbody2D _rigidbody2D; // Player's RigidBody2D component
        private readonly InputHandler _input; // Player's Input

        #endregion

        #region Constructor
        
        /// <summary>
        /// Initializes the movement of the player
        /// </summary>
        /// <param name="rigidbody2D">Player's RigidBody2D component</param>
        public PlayerMovement(Rigidbody2D rigidbody2D) {
            _rigidbody2D = rigidbody2D;
            
            
            _input = InputHandler.Instance;
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// Moves the player
        /// </summary>
        /// <param name="speed">Speed which the player will move</param>
        public void Move(float speed) {
            Vector2 movement = new Vector2(_input.GetMovement().x, _input.GetMovement().y);
            _rigidbody2D.MovePosition(_rigidbody2D.position + speed * Time.fixedDeltaTime * movement.normalized);
        }

        #endregion
    }
}
