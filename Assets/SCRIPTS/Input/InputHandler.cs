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
		[Tooltip("Mouse Position")]
		private Vector2 _mouse;
		[Tooltip("Main Camera")] 
		[SerializeField] private Camera MainCamera;

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
			OnMouseMove();
		}

		private void OnMove() {
			_movement = _input.InputCharacter.InputMovement.ReadValue<Vector2>();
		}

		private void OnMouseMove() {
			_mouse =MainCamera.ScreenToWorldPoint(_input.InputCharacter.InputShootPosition.ReadValue<Vector2>());
		}

		public Vector2 GetMovement() {
			return _movement;
		}

		public Vector2 GetMousePosition() {
			return _mouse;
		}
	}
}
