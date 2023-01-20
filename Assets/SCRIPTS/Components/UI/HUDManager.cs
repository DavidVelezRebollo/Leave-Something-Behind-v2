using UnityEngine;
using LSB.Classes.UI;
using LSB.Components.Core;
using LSB.Components.Player;
using TMPro;

namespace LSB.Components.UI {
	public class HUDManager : MonoBehaviour {
		[Header("Unity Fields")]
		[SerializeField] private TextMeshProUGUI TimerText;

		private GameManager _gameManager;
		private PlayerManager _player;

		private Timer _timer;
		
		private void Start() {
			_gameManager = GameManager.Instance;
			_player = FindObjectOfType<PlayerManager>();

			_timer = new Timer();
			_player.OnTakeDamage += UpdateHpUI;
		}

		private void Update() {
			if (_timer.GetMinuteCount() >= 30) {
				_gameManager.SetGameState(GameState.Won);
				return;
			}
			
			TimerText.text = $"{_timer.GetMinuteCount():00}:{_timer.GetSecondCount():00}";
			_timer.UpdateTimer();
		}

		private void UpdateHpUI() {
			
		}
	}
	
}
