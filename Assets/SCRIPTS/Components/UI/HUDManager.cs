using UnityEngine;
using LSB.Classes.UI;
using LSB.Components.Core;
using TMPro;

namespace LSB.Components.UI {
	public class HUDManager : MonoBehaviour {
		[Header("Unity Fields")]
		[SerializeField] private TextMeshProUGUI TimerText;

		private GameManager _gameManager;

		private Timer _timer;
		
		private void Start() {
			_gameManager = GameManager.Instance;

			_timer = new Timer();
		}

		private void Update() {
			if (_timer.GetMinuteCount() >= 30) {
				_gameManager.SetGameState(GameState.Won);
				return;
			}
			
			TimerText.text = $"{_timer.GetMinuteCount():00}:{_timer.GetSecondCount():00}";
			_timer.UpdateTimer();
		}
	}
	
}
