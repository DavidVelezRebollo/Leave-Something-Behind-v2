using UnityEngine;

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
		

		public void SetGameState(GameState state) {
			_gameState = state;
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
