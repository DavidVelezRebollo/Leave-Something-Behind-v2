using System.Collections.Generic;
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

        [Tooltip("Player Items List")]
        private List<Item> _playerItems;

        [Tooltip("Number of Normal Items which the player will have")]
        private const int _NORMAL_ITEMS_COUNT = 3;
        [Tooltip("Number of Narrative Items which the player will have")]
        private const int _NARRATIVE_ITEMS_COUNT = 2;

        private void Start() {
            _playerItems = new List<Item>();
            
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
                
                _playerItems.Add(NormalItems[index]);
                NormalItems[index].UseItem();
                NormalItems.RemoveAt(index);
            }

            // Add Narrative Items to the player's items
            for (int i = 0; i < _NARRATIVE_ITEMS_COUNT; i++) {
                int index = Random.Range(0, NarrativeItems.Count);
                
                _playerItems.Add(NarrativeItems[index]);
                NarrativeItems[index].UseItem();
                NarrativeItems.RemoveAt(index);
            }
        }
        
        /// <summary>
        /// Drops an item and undo its function
        /// </summary>
        /// <param name="itemImage">The image used to represent the item</param>
        public void DropItem(Image itemImage) {
            Item itemToDrop = _playerItems.Find(i => i.GetSprite() == itemImage.sprite);

            if (itemToDrop == null) {
                Debug.LogWarning("Item " + itemImage.name + " not found");
                return;
            }

            itemToDrop.UndoItem();
            _playerItems.Remove(itemToDrop);
        }

    }
}

