using LSB.Components.Items;
using LSB.Components.Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LSB.Shared;

namespace LSB.Components.UI {
    public class ItemSelector : MonoBehaviour {
        #region Serialize Fields

        [SerializeField] private GameObject ItemSelectorPanel;
		[Header("Left Item")]
		[SerializeField] private Image LeftItemImage;
		[SerializeField] private TextMeshProUGUI LeftItemName;
		[SerializeField] private TextMeshProUGUI LeftItemDescription;
		[Space(5)]
		[Header("Right Item")]
		[SerializeField] private Image RightItemImage;
		[SerializeField] private TextMeshProUGUI RightItemName;
		[SerializeField] private TextMeshProUGUI RightItemDescription;
		[Space(5)]
		[Header("Other Fields")]
		[SerializeField] private TextMeshProUGUI TechnicalDescription;
		[SerializeField] private Button SelectItemButton;
		[SerializeField] private GameObject Counter;

        #endregion

        #region Private Fields

        private BackPack _backPack;
		private GameManager _gameManager;
		private int _rightItem;
		private int _leftItem;
		private int _itemSelected;
		private bool _inItemSelection;
		private bool _itemDrop;
		private bool _spanish;

        #endregion

        #region Unity Events

        private void OnEnable() {
			_gameManager = GameManager.Instance;
			_backPack = BackPack.Instance;
        }

        #endregion

        #region Getters & Setters

        public void SetItemDrop(bool drop) { _itemDrop = drop; }

		public bool ItemDrop() { return _itemDrop; }

		public bool InItemSelection() { return _inItemSelection; }

        #endregion

        #region Public Methods

        public void DisplayItemSelector() {
			ItemSelectorPanel.SetActive(true);

			_gameManager.SetGameState(GameState.Paused);
			_inItemSelection = true;
			_spanish = _gameManager.GetCurrentLanguage() == Language.Spanish;

			if (_backPack.ItemsRemaining() >= 2) {
				do {
					_rightItem = Random.Range(0, _backPack.ItemsRemaining());
					_leftItem = Random.Range(0, _backPack.ItemsRemaining());
				} while (_rightItem == _leftItem);

				Item leftItem = _backPack.GetItem(_leftItem);
				Item rightItem = _backPack.GetItem(_rightItem);

				// LEFT ITEM
				LeftItemImage.sprite = leftItem.GetSprite();
				LeftItemName.text = _spanish ? leftItem.GetName() : leftItem.GetEnglishName();
				LeftItemDescription.text = _spanish ? leftItem.GetDescription() : leftItem.GetEnglishDescription();

				// RIGHT ITEM
				RightItemImage.sprite = rightItem.GetSprite();
				RightItemName.text = _spanish ? rightItem.GetName() : rightItem.GetEnglishName();
				RightItemDescription.text = _spanish ? rightItem.GetDescription() : rightItem.GetEnglishDescription();

				SelectItemButton.interactable = false;
				SelectItemButton.gameObject.GetComponent<Image>().color = new Color(0.22f, 0.22f, 0.22f);
			} else {
				_backPack.DropItem();
			}

			_itemDrop = true;
		}

		public void OnItemDrop() {
			bool leftItem = _itemSelected == _leftItem;

			_backPack.DropItem(leftItem ? _leftItem : _rightItem);
			_itemSelected = -1;
			_inItemSelection = false;

			ItemSelectorPanel.SetActive(false);
			Counter.SetActive(true);
		}

		public void OnItemSelect(bool leftItem) {
			Item itemSelect = _backPack.GetItem(leftItem ? _leftItem : _rightItem);
			TechnicalDescription.text = _spanish ? itemSelect.GetEnglishTechnicalDescription() : itemSelect.GetEnglishTechnicalDescription();
			SelectItemButton.interactable = true;
			SelectItemButton.gameObject.GetComponent<Image>().color = Color.white;
        }

        #endregion
    }
}
