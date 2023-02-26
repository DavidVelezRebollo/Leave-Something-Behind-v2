using LSB.Components.Core;
using LSB.Shared;
using LSB.Components.Player;
using UnityEngine;
using TMPro;

namespace LSB.Components.UI {
    public class Pause : MonoBehaviour {
        #region Serialized Fields

        [Header("Panels")]
		[SerializeField] private GameObject PauseMenu;
		[SerializeField] private GameObject Tutorial;
		[SerializeField] private GameObject ItemsPanel;
		[SerializeField] private GameObject Settings;
		[Space(5)]
		[Header("Stats Texts")]
		[SerializeField] private TextMeshProUGUI MaxHpText;
		[SerializeField] private TextMeshProUGUI AttackText;
		[SerializeField] private TextMeshProUGUI SpeedText;
		[SerializeField] private TextMeshProUGUI AttackSpeedText;

		#endregion

		#region Private Fields

		private GameManager _gameManager;
		private PlayerManager _player;

        #endregion

        #region Unity Events

        private void OnEnable() {
			if (_player) return;

			_player = FindObjectOfType<PlayerManager>();
        }

        #endregion

        public void ShowPause(bool show) { PauseMenu.SetActive(show); }

		public void HandlePauseMenu() {
			if(!_gameManager) _gameManager = GameManager.Instance;
			_gameManager.SetGameState(_gameManager.GamePaused() ? GameState.Running : GameState.Paused);

			ShowPause(_gameManager.GamePaused());

			ItemsPanel.SetActive(false);
			Settings.SetActive(false);
			Tutorial.SetActive(false);

			AttackSpeedText.text = _player.GetAttackSpeed().ToString("0.00");
			AttackText.text = _player.GetDamage().ToString("0");
			SpeedText.text = _player.GetSpeed().ToString("0.0");
			MaxHpText.text = _player.GetMaxHp().ToString("0");
		}
	}
}
