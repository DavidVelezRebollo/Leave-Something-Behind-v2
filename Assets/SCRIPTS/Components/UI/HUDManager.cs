using UnityEngine;
using LSB.Classes.UI;
using LSB.Input;
using LSB.Components.Core;
using LSB.Components.Player;
using LSB.Components.Items;
using LSB.Components.Audio;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

namespace LSB.Components.UI {
	public class HUDManager : MonoBehaviour {
		[Header("UI Fields")] 
		[SerializeField] private GameObject Tutorial;
		[SerializeField] private GameObject ItemContainers;
		[SerializeField] private GameObject ItemGrid;
		[SerializeField] private TextMeshProUGUI TimerText;
		[SerializeField] private GameObject Victory;
		[SerializeField] private GameObject Defeat;
		[Header("HP UI Fields")]
		[SerializeField] private Image HpBar;
		[SerializeField] private TextMeshProUGUI HpText;
		[Space(15)]
		[Header("Pause Menu UI Fields")]
		[SerializeField] private GameObject PauseMenu;
		[SerializeField] private TextMeshProUGUI MaxHpText;
		[SerializeField] private TextMeshProUGUI AttackText;
		[SerializeField] private TextMeshProUGUI SpeedText;
		[SerializeField] private TextMeshProUGUI AttackSpeedText;
		[Space(15)]
		[Header("Item Selector Fields")]
		[SerializeField] private GameObject ItemSelectorPanel;
		[SerializeField] private Image LeftItemImage;
		[SerializeField] private TextMeshProUGUI LeftItemName;
		[SerializeField] private TextMeshProUGUI LeftItemDescription;
		[SerializeField] private TextMeshProUGUI LeftItemTechnicalDescription;
		[Space(5)]
		[SerializeField] private Image RightItemImage;
		[SerializeField] private TextMeshProUGUI RightItemName;
		[SerializeField] private TextMeshProUGUI RightItemDescription;
		[SerializeField] private TextMeshProUGUI RightItemTechnicalDescription;

		[SerializeField] private GameObject ObjectExplanationPanel;

		private GameManager _gameManager;
		private PlayerManager _player;
		private BackPack _backPack;
		private InputHandler _input;
		

		private Timer _timer;
		private int _rightItem;
		private int _leftItem;
		private bool _itemDrop;

		private void Start() {
			
			_gameManager = GameManager.Instance;
			_player = FindObjectOfType<PlayerManager>();
			_backPack = BackPack.Instance;
			_input = InputHandler.Instance;

			if (!PlayerPrefs.HasKey("Tutorial")) {
				_gameManager.SetGameState(GameState.Paused);
				Tutorial.SetActive(true);
			} else if (PlayerPrefs.GetInt("Tutorial") == 1) {
				_gameManager.SetGameState(GameState.Paused);
				Tutorial.SetActive(true);
			}
			else {
				_gameManager.SetGameState(GameState.Running);
				Tutorial.SetActive(false);
			}

			_timer = new Timer(12);
			_player.OnHpChange += updateHpUI;
			_backPack.OnItemInitialize += () => { HpText.text = Mathf.FloorToInt(_player.GetMaxHp()).ToString(); };
		}

		private void Update() {
			if (_gameManager.GetGameState() == GameState.Lost) { handleDefeat(); return; }
			if (_gameManager.GetGameState() == GameState.Won) { handleVictory(); return; }

			if (_input.OnPauseButton()) handlePauseMenu();
			if (_gameManager.GamePaused() || _gameManager.GameEnded()) return;
			
			_timer.UpdateTimer();
			
			TimerText.text = $"{_timer.GetMinuteCount():00}:{_timer.GetSecondCount():00}";
			
			if (_timer.GetMinuteCount() <= 0) {
				_gameManager.SetGameState(GameState.Won);
				return;
			}

			if (_timer.GetMinuteCount() % 2 != 0 && _timer.GetSecondCount() != 0) _itemDrop = false;

			if (_timer.GetMinuteCount() % 2 != 0 || _timer.GetSecondCount() != 0
			    || _timer.GetMinuteCount() ==  _timer.GetTotalMinutes() || _itemDrop) return;

			if (_backPack.ItemsRemaining() != 6) DisplayItemSelector();
			else {
				_gameManager.SetGameState(GameState.Paused);
				ObjectExplanationPanel.SetActive(true);
			}
		}
		

		public void DisplayItemSelector() {
			_gameManager.SetGameState(GameState.Paused);
			
			ItemSelectorPanel.SetActive(true);
			ItemContainers.SetActive(false);
			ItemGrid.SetActive(false);
			
			if (_backPack.ItemsRemaining() > 1) {
				do {
					_rightItem = Random.Range(0, _backPack.ItemsRemaining());
					_leftItem = Random.Range(0, _backPack.ItemsRemaining());
				} while (_rightItem == _leftItem || !_backPack.ExistItem(_rightItem) || !_backPack.ExistItem(_leftItem));
				
				Item leftItem = _backPack.GetItem(_leftItem);
				Item rightItem = _backPack.GetItem(_rightItem);

				// LEFT ITEM
				LeftItemImage.sprite = leftItem.GetSprite();
				LeftItemName.text = leftItem.GetName();
				LeftItemDescription.text = leftItem.GetDescription();
				LeftItemTechnicalDescription.text = leftItem.GetTechnicalDescription();
				
				// RIGHT ITEM
				RightItemImage.sprite = rightItem.GetSprite();
				RightItemName.text = rightItem.GetName();
				RightItemDescription.text = rightItem.GetDescription();
				RightItemTechnicalDescription.text = rightItem.GetTechnicalDescription();
			}
			else {
				_backPack.DropItem();
			}

			_itemDrop = true;
		}

		public void OnLeftItemSelect() {
			_backPack.DropItem(_leftItem);
			_gameManager.SetGameState(GameState.Running);
			ItemSelectorPanel.SetActive(false);
			ItemContainers.SetActive(true);
			ItemGrid.SetActive(true);
		}

		public void OnRightItemSelect() {
			_backPack.DropItem(_rightItem);
			_gameManager.SetGameState(GameState.Running);
			ItemSelectorPanel.SetActive(false);
			ItemContainers.SetActive(true);
			ItemGrid.SetActive(true);
		}

		public float GetMinutes() {
			return _timer.GetMinuteCount();
		}

		public int GetSeconds() {
			return _timer.GetSecondCount();
		}

		private void updateHpUI() {
			float playerCurrentHp = _player.GetCurrentHp();
			float playerMaxHp = _player.GetMaxHp();
			
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

		private void handlePauseMenu() {
			_gameManager.SetGameState(_gameManager.GamePaused() ? GameState.Running : GameState.Paused);
			
			PauseMenu.SetActive(!PauseMenu.activeSelf);
			ItemContainers.SetActive(!ItemContainers.activeSelf);
			ItemGrid.SetActive(!ItemContainers.activeSelf);
			TimerText.enabled = !TimerText.IsActive();

			AttackSpeedText.text = _player.GetAttackSpeed().ToString();
			AttackText.text = _player.GetDamage().ToString();
			SpeedText.text = _player.GetSpeed().ToString();
			MaxHpText.text = _player.GetMaxHp().ToString();
		}

		public void ShowTutorialNextTime(bool show) {
			PlayerPrefs.SetInt("Tutorial", !show ? 1 : 0);
			PlayButtonSound();
		}

		public void OnStartButton() {
			Tutorial.SetActive(false);
			_gameManager.SetGameState(GameState.Running);
		}

		public void OnContinueButton() {
			_gameManager.SetGameState(GameState.Running);
			
			PauseMenu.SetActive(false);
			ItemContainers.SetActive(true);
			ItemGrid.SetActive(true);
			TimerText.enabled = true;
		}

		public void OnExitButton()
		{
			GameManager.Instance.SetGameState(GameState.Menu);
			SceneManager.LoadScene(0);
		}


		private void handleVictory()
		{
			_gameManager.SetGameState(GameState.Menu);
			Victory.SetActive(true);
		}

		private void handleDefeat()
		{
			_gameManager.SetGameState(GameState.Menu);
			Defeat.SetActive(true);
		}

		public void PlayButtonSound() { SoundManager.Instance.PlayOneShot("Button"); }
	}
	
}
