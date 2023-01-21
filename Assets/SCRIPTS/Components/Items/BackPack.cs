using System;
using System.Collections.Generic;
using System.Linq;
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
        [Tooltip("Narrative Items List")]
        [SerializeField] private List<Item> NarrativeItems;
        [SerializeField] private GameObject ItemContainer;

        public Action OnItemInitialize;

        [Tooltip("Player Items List")]
        private Dictionary<int, Item> _playerItems;

        [Tooltip("Number of Normal Items which the player will have")]
        private const int _NORMAL_ITEMS_COUNT = 4;
        [Tooltip("Number of Narrative Items which the player will have")]
        private const int _NARRATIVE_ITEMS_COUNT = 2;

        private void Start() {
            _playerItems = new Dictionary<int, Item>();
            fillBackPack();
        }

        /// <summary>
        /// Fills the player's items with items
        /// </summary>
        private void fillBackPack() {
            if (NormalItems.Count < _NORMAL_ITEMS_COUNT && NarrativeItems.Count < _NARRATIVE_ITEMS_COUNT) {
                Debug.LogError("Not enough items to fill the backpack");
                return;
            }
            
            // Add Normal Items to the player's items
            for (int i = 0; i < _NORMAL_ITEMS_COUNT; i++) {
                int index = Random.Range(0, NormalItems.Count);
                
                _playerItems.Add(i, NormalItems[index]);
                NormalItems[index].UseItem();
                ItemContainer.gameObject.transform.GetChild(i).GetComponent<Image>().sprite = NormalItems[index].GetSprite();
                NormalItems.RemoveAt(index);
            }

            // Add Narrative Items to the player's items
            for (int i = 0; i < _NARRATIVE_ITEMS_COUNT; i++) {
                int index = Random.Range(0, NarrativeItems.Count);
                
                _playerItems.Add(i + _NORMAL_ITEMS_COUNT, NarrativeItems[index]);
                NarrativeItems[index].UseItem();
                ItemContainer.gameObject.transform.GetChild(i + _NORMAL_ITEMS_COUNT).GetComponent<Image>().sprite = NarrativeItems[index].GetSprite();
                NarrativeItems.RemoveAt(index);
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
                _playerItems.Last().Value.UndoItem();
                _playerItems.Remove(_playerItems.Last().Key);
                return;
            }
            
            Item itemToDrop = _playerItems[index];

            if (itemToDrop == null) {
                Debug.LogError("Item with index" + index + " not found");
                return;
            }

            itemToDrop.UndoItem();
            ItemContainer.gameObject.transform.GetChild(index).GetComponent<Image>().color = Color.black;
            _playerItems.Remove(index);
        }

    }
}

