using System;
using UnityEngine;

namespace LSB.Input {
	public class InputHandler : MonoBehaviour {
		[Tooltip("Singleton Instance")] 
		public static InputHandler Instance;
		[Tooltip("Input System Variable")]
		private GameInput _input;
		[Tooltip("Movement Vector")]
		private Vector2 _movement;

		private void Awake() {
			if (_input == null) _input = new GameInput();
			if (Instance != null) return;

			Instance = this;

			DontDestroyOnLoad(this);
		}

		private void OnEnable() {
			_input.Enable();
		}

		private void OnDisable() {
			_input.Disable();
		}

		private void Update() {
			OnMove();
		}

		private void OnMove() {
			_movement = _input.InputCharacter.InputMovement.ReadValue<Vector2>();
			Debug.Log(_movement);
		}

		public Vector2 GetMovement() {
			return _movement;
		}
	}
}
