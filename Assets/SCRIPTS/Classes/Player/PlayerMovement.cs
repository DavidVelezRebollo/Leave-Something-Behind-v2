using LSB.Interfaces;
using LSB.Input;
using UnityEngine;

namespace LSB.Classes.Player {
    public class PlayerMovement {
        private readonly CharacterController _characterController;
        private float _speed;
        private readonly InputHandler _input;
        
        public PlayerMovement(CharacterController controller, float speed) {
            _characterController = controller;
            _speed = speed;
            
            _input = InputHandler.Instance;
        }
        
        public void Move() {
            _characterController.Move(new Vector3(_input.GetMovement().x, _input.GetMovement().y, 0) * (_speed * Time.fixedDeltaTime));
        }

        public void AddSpeed(float speed)
        {
            _speed *= speed;
        }
    }
}
