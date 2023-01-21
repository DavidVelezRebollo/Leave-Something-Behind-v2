using UnityEngine;
using LSB.Classes.UI;
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
		[Header("Item Selector Fields")]
		[SerializeField] private GameObject ItemSelectorPanel;
		[SerializeField] private Image LeftItemImage;
		[SerializeField] private TextMeshProUGUI LeftItemName;
		[SerializeField] private TextMeshProUGUI LeftItemDescription;
		[SerializeField] private TextMeshProUGUI LeftItemTechnicalDescription;
		[SerializeField] private Image RightItemImage;
		[SerializeField] private TextMeshProUGUI RightItemName;
		[SerializeField] private TextMeshProUGUI RightItemDescription;
		[SerializeField] private TextMeshProUGUI RightItemTechnicalDescription;

		private GameManager _gameManager;
		private PlayerManager _player;
		private BackPack _backPack;

		private Timer _timer;
		private int _rightItem;
		private int _leftItem;
		private bool _itemDrop;
		
		private void Start() {
			_gameManager = GameManager.Instance;
			_player = FindObjectOfType<PlayerManager>();
			_backPack = BackPack.Instance;

			_timer = new Timer();
			_player.OnTakeDamage += updateHpUI;
		}

		private void Update() {
			if (_timer.GetMinuteCount() >= 30) {
				_gameManager.SetGameState(GameState.Won);
				return;
			}

			if (_timer.GetMinuteCount() % 5 != 0) _itemDrop = false;

			if (_timer.GetMinuteCount() % 5 == 0 && !_itemDrop) {
				displayItemSelector();
				_itemDrop = true;
			}
			
			TimerText.text = $"{_timer.GetMinuteCount():00}:{_timer.GetSecondCount():00}";
			
			if(!_gameManager.GamePaused() && !_gameManager.GameEnded()) _timer.UpdateTimer();
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
	}
	
}
