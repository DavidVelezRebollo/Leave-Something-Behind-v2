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

		public GameState GetGameState()
		{
			return _gameState;
		}

		public void SetGameState(GameState state) {
			_gameState = state;
			if (state == GameState.Paused || state == GameState.Menu)
			{
				Cursor.SetCursor(HandleCursor, new Vector2(0, 0), CursorMode.Auto);
				SoundManager.Instance.Play("MenuSong");
				SoundManager.Instance.Stop("ThemeSong");
			}
			else if (state == GameState.Running)
			{
				Cursor.SetCursor(TargetCursor, new Vector2(0, 0), CursorMode.Auto);
				SoundManager.Instance.Play("ThemeSong");
				SoundManager.Instance.Stop("MenuSong");
			}
		}

		public bool GameEnded() { return _gameState == GameState.Lost || _gameState == GameState.Won; }

		public bool GamePaused() { return _gameState == GameState.Paused; }

		/*public void ResetGame()
        {
			OrcCurrentStats.Damage = OrcBaseStats.Damage;
			OrcCurrentStats.Speed = OrcBaseStats.Speed;
			OrcCurrentStats.MaxHp = OrcBaseStats.MaxHp;
		}
		*/
	}
}
