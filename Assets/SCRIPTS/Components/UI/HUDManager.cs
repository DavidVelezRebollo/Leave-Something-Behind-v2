using UnityEngine;
using LSB.Classes.UI;
using LSB.Input;
using LSB.Components.Core;
using LSB.Components.Player;
using LSB.Components.Items;
using TMPro;
using UnityEngine.UI;

namespace LSB.Components.UI {
	public class HUDManager : MonoBehaviour {
		[Header("UI Fields")] 
		[SerializeField] private GameObject ItemContainers;
		[SerializeField] private TextMeshProUGUI TimerText;
		[SerializeField] private GameObject PauseMenu;
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

			_timer = new Timer();
			_player.OnTakeDamage += updateHpUI;
		}

		private void Update() {
			if(_input.OnPauseButton()) handlePauseMenu();
			if (_gameManager.GamePaused() || _gameManager.GameEnded()) return;
			
			TimerText.text = $"{_timer.GetMinuteCount():00}:{_timer.GetSecondCount():00}";
			
			_timer.UpdateTimer();
			
			if (_timer.GetMinuteCount() >= 30) {
				_gameManager.SetGameState(GameState.Won);
				return;
			}

			if (_timer.GetMinuteCount() % 5 != 0) _itemDrop = false;

			if (_timer.GetMinuteCount() % 5 != 0 || _timer.GetMinuteCount() == 0 || _itemDrop) return;
			
			displayItemSelector();
			_itemDrop = true;
			
		}

		private void displayItemSelector() {
			GameManager.Instance.SetGameState(GameState.Paused);
			
			ItemSelectorPanel.SetActive(true);
			ItemContainers.SetActive(false);
			if (_backPack.ItemsRemaining() > 1) {
				do {
					_rightItem = Random.Range(0, _backPack.ItemsRemaining());
					_leftItem = Random.Range(0, _backPack.ItemsRemaining());
				} while (_rightItem == _leftItem);
				
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
		}

		public void OnLeftItemSelect() {
			_backPack.DropItem(_leftItem);
			_gameManager.SetGameState(GameState.Running);
			ItemSelectorPanel.SetActive(false);
			ItemContainers.SetActive(true);
		}

		public void OnRightItemSelect() {
			_backPack.DropItem(_rightItem);
			_gameManager.SetGameState(GameState.Running);
			ItemSelectorPanel.SetActive(false);
			ItemContainers.SetActive(true);
		}

		private void updateHpUI() {
			
		}

		private void handlePauseMenu() {
			_gameManager.SetGameState(_gameManager.GamePaused() ? GameState.Running : GameState.Paused);
			
			PauseMenu.SetActive(!PauseMenu.activeSelf);
			ItemContainers.SetActive(!ItemContainers.activeSelf);
			TimerText.enabled = !TimerText.IsActive();
		}

		public void OnContinueButton() {
			_gameManager.SetGameState(GameState.Running);
			
			PauseMenu.SetActive(false);
			ItemContainers.SetActive(true);
			TimerText.enabled = true;
		}
	}
	
}
