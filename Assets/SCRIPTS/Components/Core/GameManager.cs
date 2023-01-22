using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

		public static GameManager Instance;

		private void Awake() {
			if (Instance != null) return;

			Instance = this;

			_gameState = GameState.Menu;
		}

		#endregion

		private GameState _gameState;
		[SerializeField] private Texture2D HandleCursor;
		[SerializeField] private Texture2D TargetCursor;

		public void Update() {
			if (!GameEnded()) return;
			
			if(_gameState == GameState.Lost) handleDefeat();
			if(_gameState == GameState.Won) handleVictory();
		}

		public void SetGameState(GameState state) {
			_gameState = state;
			if(state == GameState.Paused || state == GameState.Menu) Cursor.SetCursor(HandleCursor, new Vector2(0, 0), CursorMode.Auto);
			else if(state == GameState.Running) Cursor.SetCursor(TargetCursor, new Vector2(0, 0), CursorMode.Auto);
		}

		public bool GameEnded() { return _gameState == GameState.Lost || _gameState == GameState.Won; }

		public bool GamePaused() { return _gameState == GameState.Paused; }

		private void handleVictory() {
			Debug.LogError("TO DO - Victory Scene");
			SetGameState(GameState.Menu);
		}

		private void handleDefeat() {
			Debug.LogError("TO DO - Defeat Scene");
			SetGameState(GameState.Menu);
		}

		/*public void ResetGame()
        {
			OrcCurrentStats.Damage = OrcBaseStats.Damage;
			OrcCurrentStats.Speed = OrcBaseStats.Speed;
			OrcCurrentStats.MaxHp = OrcBaseStats.MaxHp;
		}
		*/
	}
}
