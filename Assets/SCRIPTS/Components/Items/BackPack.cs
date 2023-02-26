using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace LSB.Components.Items {
    public class BackPack : MonoBehaviour {
        #region Singleton

        public static BackPack Instance;
        
        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;
        }

        #endregion

        [Tooltip("Normal Items List")]
        [SerializeField] private List<Item> NormalItems;
        [Space(5)]
        [Tooltip("UI Fields")]
        [SerializeField] private List<Image> ItemIcons;
        [SerializeField] private List<Image> ItemMenuIcons;
        [SerializeField] private List<TextMeshProUGUI> ItemMenuTexts;

        public Action OnItemInitialize;

        [Tooltip("Player Items List")]
        private List<Item> _playerItems;

        [Tooltip("Number of Normal Items which the player will have")]
        private const int _ITEMS_COUNT = 6;

        private void Start() {
            _playerItems = new List<Item>();
            fillBackPack();
        }

        /// <summary>
        /// Fills the player's items with items
        /// </summary>
        private void fillBackPack() {
            if (NormalItems.Count < _ITEMS_COUNT) {
                Debug.LogError("Not enough items to fill the backpack");
                return;
            }
            
            // Add Normal Items to the player's items
            for (int i = 0; i < _ITEMS_COUNT; i++) {
                int index = Random.Range(0, NormalItems.Count);
                
                _playerItems.Add(NormalItems[index]);
                NormalItems[index].UseItem();
                ItemIcons[i].sprite = NormalItems[index].GetSprite();
                NormalItems.RemoveAt(index);
            }

            OnItemInitialize?.Invoke();
        }

        public int ItemsRemaining() { return _playerItems.Count; }

        public Item GetItem(int index) { return _playerItems[index]; }
        
        /// <summary>
        /// Drops an item and undo its function
        /// </summary>
        /// <param name="index">The key used to identify the item</param>
        public void DropItem(int index = 0) {
            if (_playerItems.Count == 1) {
                _playerItems.Last().UndoItem();
                _playerItems.Remove(_playerItems.Last());
                return;
            }
            
            Item itemToDrop = _playerItems[index];

            if (itemToDrop == null) {
                Debug.LogError("Item with index" + index + " not found");
                return;
            }

            Debug.Log(index);

            itemToDrop.UndoItem();
            ItemIcons[index].color = Color.black;
            ItemMenuIcons[index].color = Color.black;
            ItemMenuTexts[index].fontStyle = FontStyles.Strikethrough;
            _playerItems.Remove(itemToDrop);
            ItemIcons.RemoveAt(index);
            ItemMenuIcons.RemoveAt(index);
            ItemMenuTexts.RemoveAt(index);
        }

    }
}

