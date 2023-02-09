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
		[Header("UI Fields")] 
		[SerializeField] private GameObject Tutorial;
		[SerializeField] private GameObject InitialTutorial;
		[SerializeField] private GameObject ItemsPanel;
		[SerializeField] private GameObject Settings;
		[SerializeField] private GameObject ItemContainers;
		[SerializeField] private GameObject ItemGrid;
		[SerializeField] private GameObject ItemPanelGrid;
		[SerializeField] private TextMeshProUGUI TimerText;
		[SerializeField] private GameObject Victory;
		[SerializeField] private GameObject Defeat;
		[Header("HP & Energy UI Fields")]
		[SerializeField] private Image HpBar;
		[SerializeField] private TextMeshProUGUI HpText;
		[SerializeField] private Image EnergyBar;
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
		[Space(5)]
		[SerializeField] private Image RightItemImage;
		[SerializeField] private TextMeshProUGUI RightItemName;
		[SerializeField] private TextMeshProUGUI RightItemDescription;
		[Space(1)]
		[SerializeField] private TextMeshProUGUI TechnicalDescription;
		[SerializeField] private Button SelectItemButton;

		[SerializeField] private GameObject ObjectExplanationPanel;

		private GameManager _gameManager;
		private PlayerManager _player;
		private BackPack _backPack;
		private InputHandler _input;

		private Timer _timer;
		private int _rightItem;
		private int _leftItem;
		private int _itemSelected;
		private bool _itemDrop;
		private bool _timerBlink;
		private bool _rechargeEnergy;

		private void Start() {
			
			_gameManager = GameManager.Instance;
			_player = FindObjectOfType<PlayerManager>();
			_backPack = BackPack.Instance;
			_input = InputHandler.Instance;

			if (!PlayerPrefs.HasKey("Tutorial")) {
				_gameManager.SetGameState(GameState.Paused);
				InitialTutorial.SetActive(true);
			} else if (PlayerPrefs.GetInt("Tutorial") == 1) {
				_gameManager.SetGameState(GameState.Paused);
				InitialTutorial.SetActive(true);
			}
			else {
				_gameManager.SetGameState(GameState.Running);
				InitialTutorial.SetActive(false);
			}

			_timer = new Timer(12);
			_player.OnHpChange += updateHpUI;
			_player.SubscribeSpecialAttack(() => { _rechargeEnergy = true; });
			_backPack.OnItemInitialize += () => {
				HpText.text = Mathf.FloorToInt(_player.GetMaxHp()).ToString();
				initializeItemPanel();
			};
		}

		private void Update() {
			if (_gameManager.GetGameState() == GameState.Lost) { handleDefeat(); return; }
			if (_gameManager.GetGameState() == GameState.Won) { handleVictory(); return; }

			if (_input.OnPauseButton()) handlePauseMenu();
			if (_gameManager.GamePaused() || _gameManager.GameEnded()) return;
			
			_timer.UpdateTimer();
			
			handleTimerText();

			EnergyBar.fillAmount = _player.GetCurrentEnergyAmount() / _player.GetTotalEnergy();
			
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
				
				// RIGHT ITEM
				RightItemImage.sprite = rightItem.GetSprite();
				RightItemName.text = rightItem.GetName();
				RightItemDescription.text = rightItem.GetDescription();

				SelectItemButton.interactable = false;
				SelectItemButton.gameObject.GetComponent<Image>().color = new Color(0.22f, 0.22f, 0.22f);
			}
			else {
				_backPack.DropItem();
			}

			_itemDrop = true;
		}

		public void OnLeftItemSelect() {
			Item leftItem = _backPack.GetItem(_leftItem);
			TechnicalDescription.text = leftItem.GetTechnicalDescription();
			_itemSelected = _leftItem;
			SelectItemButton.interactable = true;
			SelectItemButton.gameObject.GetComponent<Image>().color = Color.white;
		}

		public void OnRightItemSelect() {
			Item rightItem = _backPack.GetItem(_rightItem);
			TechnicalDescription.text = rightItem.GetTechnicalDescription();
			_itemSelected = _rightItem;
			SelectItemButton.interactable = true;
			SelectItemButton.gameObject.GetComponent<Image>().color = Color.white;
		}

		public void OnItemSelect()
        {
            if (_itemSelected == _leftItem)
            {
				_backPack.DropItem(_leftItem);
				_gameManager.SetGameState(GameState.Running);
				ItemSelectorPanel.SetActive(false);
				ItemContainers.SetActive(true);
				ItemGrid.SetActive(true);
				_itemSelected = -1;
			}
			else if(_itemSelected == _rightItem)
            {
				_backPack.DropItem(_rightItem);
				_gameManager.SetGameState(GameState.Running);
				ItemSelectorPanel.SetActive(false);
				ItemContainers.SetActive(true);
				ItemGrid.SetActive(true);
				_itemSelected = -1;
			}
        }

		public float GetMinutes() {
			return _timer.GetMinuteCount();
		}

		public int GetSeconds() {
			return _timer.GetSecondCount();
		}

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

		private void handlePauseMenu() {
			_gameManager.SetGameState(_gameManager.GamePaused() ? GameState.Running : GameState.Paused);
			
			PauseMenu.SetActive(_gameManager.GamePaused());
			ItemContainers.SetActive(!_gameManager.GamePaused());
			ItemGrid.SetActive(!_gameManager.GamePaused());
			TimerText.enabled = !TimerText.IsActive();
			
			ItemsPanel.SetActive(false);
			Settings.SetActive(false);
			Tutorial.SetActive(false);

			AttackSpeedText.text = _player.GetAttackSpeed().ToString("0.00");
			AttackText.text = _player.GetDamage().ToString("0");
			SpeedText.text = _player.GetSpeed().ToString("0.0");
			MaxHpText.text = _player.GetMaxHp().ToString("0");
		}

		public void ShowTutorialNextTime(bool show) {
			PlayerPrefs.SetInt("Tutorial", !show ? 1 : 0);
			PlayButtonSound();
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

		private void initializeItemPanel() {
			for (int i = 0; i < ItemPanelGrid.transform.childCount; i++) {
				ItemPanelGrid.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite =
					_backPack.GetItem(i).GetSprite();
				ItemPanelGrid.transform.transform.GetChild(i).GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text =
					_backPack.GetItem(i).GetTechnicalDescription();
			}
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

		private void handleTimerText() {
			if (_timer.GetMinuteCount() > 9) TimerText.color = new Color(0.82f, 0.94f, 0.93f);
			else if (_timer.GetMinuteCount() <= 9 && _timer.GetMinuteCount() > 5) {
				TimerText.color = new Color(0.78f, 0.65f, 0.91f);
			} else if (_timer.GetMinuteCount() <= 5 && _timer.GetMinuteCount() > 1) {
				TimerText.color = new Color(0.89f, 0.5f, 0.84f);
			} else if (_timer.GetMinuteCount() <= 1 && !_timerBlink) {
				_timerBlink = true;
				StartCoroutine(timerBlink());
			}
			
			// TODO - Timer blink
			
			TimerText.text = $"{_timer.GetMinuteCount():00}:{_timer.GetSecondCount():00}";
		}

		private IEnumerator timerBlink() {
			TimerText.color = new Color(0.88f, 0.22f, 0.31f);
			yield return new WaitForSeconds(1f);
			TimerText.color = new Color(0.82f, 0.94f, 0.93f);
			yield return new WaitForSeconds(1f);
			_timerBlink = false;
		}

		public void PlayButtonSound() { SoundManager.Instance.PlayOneShot("Button"); }
	}
	
}
