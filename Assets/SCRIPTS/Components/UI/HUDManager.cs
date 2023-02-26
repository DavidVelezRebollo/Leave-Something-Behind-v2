using System.Collections;
using UnityEngine;
using LSB.Classes.UI;
using LSB.Input;
using LSB.Components.Core;
using LSB.Components.Player;
using LSB.Components.Items;
using LSB.Components.Audio;
using LSB.Shared;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

namespace LSB.Components.UI {
	public class HUDManager : MonoBehaviour {
        #region Serialize Fields

        [Header("UI Fields")] 
		[SerializeField] private GameObject InitialTutorial;
		[SerializeField] private GameObject ItemContainers;
		[SerializeField] private GameObject ItemGrid;
		[SerializeField] private GameObject ItemPanelGrid;
		[SerializeField] private GameObject TimerContainer;
		[SerializeField] private GameObject Victory;
		[SerializeField] private GameObject Defeat;
		[SerializeField] private GameObject ObjectExplanationPanel;
		[SerializeField] private TextMeshProUGUI TimerText;
		[Space(15)]
		[Header("HP & Energy UI Fields")]
		[SerializeField] private Image HpBar;
		[SerializeField] private TextMeshProUGUI HpText;
		[SerializeField] private Image EnergyBar;
		[Space(15)]
		[Header("UI Scripts")]
		[SerializeField] private ItemSelector ItemSelector;
		[SerializeField] private Pause Pause;

        #endregion

        #region Private Fields

        private GameManager _gameManager;
		private PlayerManager _player;
		private BackPack _backPack;
		private InputHandler _input;

		private Timer _timer;
		private bool _timerBlink;
		private bool _spanish;

        #endregion

        #region Unity Events

        private void Start() {
			_gameManager = GameManager.Instance;
			_player = FindObjectOfType<PlayerManager>();
			_backPack = BackPack.Instance;
			_input = InputHandler.Instance;
			_timer = new Timer(12);

			if (!PlayerPrefs.HasKey("Tutorial") || PlayerPrefs.GetInt("Tutorial") == 1) {
				_gameManager.SetGameState(GameState.Paused);
				InitialTutorial.SetActive(true);
			} else {
				_gameManager.SetGameState(GameState.Running);
				InitialTutorial.SetActive(false);
			}

			_player.OnHpChange += updateHpUI;
			_backPack.OnItemInitialize += () => {
				HpText.text = Mathf.FloorToInt(_player.GetMaxHp()).ToString();
				initializeItemPanel();
			};
		}

		private void Update() {
			if (handleEndStates()) return;

			if (_input.OnPauseButton() && !ItemSelector.InItemSelection()) {
				Pause.HandlePauseMenu();
				TimerContainer.SetActive(!_gameManager.GamePaused());
				ShowUiItems(!_gameManager.GamePaused());
			}
			if (_gameManager.GamePaused()) return;

			_timer.UpdateTimer();

			handleTimerText();

			EnergyBar.fillAmount = _player.GetCurrentEnergyAmount() / _player.GetTotalEnergy();

			if (_timer.GetMinuteCount() <= 0 && _timer.GetSecondCount() <= 0) {
				_gameManager.SetGameState(GameState.Won);
				return;
			}

			if (_timer.GetMinuteCount() % 1 != 0 && _timer.GetSecondCount() != 0) ItemSelector.SetItemDrop(false);

			if (_timer.GetMinuteCount() % 1 != 0 || _timer.GetSecondCount() != 0 || _timer.GetMinuteCount() == _timer.GetTotalMinutes() 
				|| ItemSelector.ItemDrop()) return;

			if (_backPack.ItemsRemaining() != 6 && _backPack.ItemsRemaining() > 0) {
				ShowUiItems(false);
				ItemSelector.DisplayItemSelector();
			} else {
				_gameManager.SetGameState(GameState.Paused);
				ObjectExplanationPanel.SetActive(true);
			}
		}

        #endregion

        #region Getters

		/// <summary>
		/// Gets the current minutes of the timer
		/// </summary>
		/// <returns>The current minutes of the timer</returns>
        public float GetMinutes() {
			return _timer.GetMinuteCount();
		}

		/// <summary>
		/// Gets the current seconds of the timer
		/// </summary>
		/// <returns>The current seconds of the timer</returns>
		public int GetSeconds() {
			return _timer.GetSecondCount();
		}

		#endregion

		#region Public Methods

		public void ShowTutorialNextTime(bool show) {
			PlayerPrefs.SetInt("Tutorial", !show ? 1 : 0);
			PlayButtonSound();
		}

		public void OnContinueButton() {
			_gameManager.SetGameState(GameState.Running);

			Pause.ShowPause(false);
			ShowUiItems(true);
			TimerText.enabled = true;
		}

		public void OnExitButton() {
			GameManager.Instance.SetGameState(GameState.Menu);
			StartCoroutine(loadSceneAsync());
		}

		public void PlayButtonSound() { SoundManager.Instance.PlayOneShot("Button"); }

		public void ShowUiItems(bool show) {
			ItemContainers.SetActive(show);
			ItemGrid.SetActive(show);
		}

		#endregion

		#region Auxiliar Methods

		private void updateHpUI() {
			float playerMaxHp = _player.GetMaxHp();
			float playerCurrentHp = _player.GetCurrentHp();

			HpText.text = Mathf.FloorToInt(playerCurrentHp).ToString();
			HpBar.fillAmount = playerCurrentHp / playerMaxHp;

			switch (HpBar.fillAmount) {
				case <= 0.75f and > 0.5f:
					HpBar.color = Color.yellow;
					break;
				case <= 0.5f and > 0.25f:
					HpBar.color = new Color(1f, 0.5f, 0f);
					break;
				case <= 0.25f:
					HpBar.color = Color.red;
					break;
				default:
					HpBar.color = Color.green;
					break;
			}
		}

		private void handleTimerText() {
			if (_timer.GetMinuteCount() > 9)
				TimerText.color = new Color(0.82f, 0.94f, 0.93f);
			else if (_timer.GetMinuteCount() <= 9 && _timer.GetMinuteCount() > 5) {
				TimerText.color = new Color(0.78f, 0.65f, 0.91f);
			} else if (_timer.GetMinuteCount() <= 5 && _timer.GetMinuteCount() > 1) {
				TimerText.color = new Color(0.89f, 0.5f, 0.84f);
			} else if (_timer.GetMinuteCount() <= 1 && !_timerBlink) {
				_timerBlink = true;
				StartCoroutine(timerBlink());
			}

			TimerText.text = $"{_timer.GetMinuteCount():00}:{_timer.GetSecondCount():00}";
		}

		private bool handleEndStates() {
			bool end = false;

			if (_gameManager.GetGameState() == GameState.Lost) {
				_gameManager.SetGameState(GameState.Paused);
				Victory.SetActive(true);
				end = true;
			}

			if (_gameManager.GetGameState() == GameState.Won) {
				_gameManager.SetGameState(GameState.Paused);
				Defeat.SetActive(true);
				end = true;
			}

			return end;
		}

		private void initializeItemPanel() {
			_spanish = _gameManager.GetCurrentLanguage() == Language.Spanish;

			for (int i = 0; i < ItemPanelGrid.transform.childCount; i++) {
				ItemPanelGrid.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite =
					_backPack.GetItem(i).GetSprite();
				ItemPanelGrid.transform.transform.GetChild(i).GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text =
					_spanish ? _backPack.GetItem(i).GetTechnicalDescription() : _backPack.GetItem(i).GetEnglishTechnicalDescription();
			}
		}

		private IEnumerator loadSceneAsync() {
			AsyncOperation loadScene = SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);

			while (!loadScene.isDone) {
				yield return null;
			}
		}

		private IEnumerator timerBlink() {
			TimerText.color = new Color(0.88f, 0.22f, 0.31f);
			yield return new WaitForSeconds(1f);
			TimerText.color = new Color(0.82f, 0.94f, 0.93f);
			yield return new WaitForSeconds(1f);
			_timerBlink = false;
		}

		#endregion
	}
}
