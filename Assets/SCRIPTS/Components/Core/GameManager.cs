using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using LSB.Components.Audio;

namespace LSB.Components.Core {
	public enum GameState {
		Menu,
		Paused,
		Running,
		Lost,
		Won
	}
	
	public class GameManager : MonoBehaviour {
		#region Singleton
		private GameState _gameState;
		public static GameManager Instance;

		private void Awake() {
			if (Instance != null) return;

			Instance = this;

			_gameState = GameState.Menu;
		}

		#endregion

		
		[SerializeField] private Texture2D HandleCursor;
		[SerializeField] private Texture2D TargetCursor;

		private SoundManager _soundManager;

		private void Start() {
			_soundManager = SoundManager.Instance;
		}

		public GameState GetGameState()
		{
			return _gameState;
		}

		public void SetGameState(GameState state) {
			_gameState = state;
			if (state == GameState.Paused || state == GameState.Menu) {
				Cursor.SetCursor(HandleCursor, new Vector2(0, 0), CursorMode.Auto);
				_soundManager.Play("MenuSong");
				
				if(state == GameState.Paused) _soundManager.Pause("ThemeSong");
				else _soundManager.Stop("ThemeSong");
			}
			else if (state == GameState.Running) {
				Cursor.SetCursor(TargetCursor, new Vector2(0, 0), CursorMode.Auto);
				
				if(!_soundManager.IsPlaying("ThemeSong")) _soundManager.Play("ThemeSong");
				else _soundManager.Resume("ThemeSong");
				
				_soundManager.Stop("MenuSong");
			}
		}

		public bool GameEnded() { return _gameState == GameState.Lost || _gameState == GameState.Won; }

		public bool GamePaused() { return _gameState == GameState.Paused; }
	}
}
