using System.Collections.Generic;
using UnityEngine;
using LSB.Components.Player;

namespace LSB.Components.Items {
    public class BackPack : MonoBehaviour {
        public static BackPack Instance;
        private PlayerManager _player;
        private List<Item> _normalItems;
        private List<Item> _narrativeItems;
        private List<Item> _playerItems;

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;
        }
        private void Start()
        {
            _player = FindObjectOfType<PlayerManager>();
        }
        public void DropItem(Sprite sprite) {
            //TODO- implement DropItem
        }
        public void FillBackPack() {
            //TODO- implement FillBackPack
        }
        public void ApplyItemFunction() {
            //TODO- implement ApplyItemFunction
        }
        public void EmptyBackPack() {
            //TODO- implement EmptyBackPack
        }
        public void ClearItemFunction() {
            //TODO- implement ClearItemFunction
        }

        #region Getters&Setters
        public void AddNormalItem(Item item)
        {
            _normalItems.Add(item);
        }
        public void AddNarrativeItem(Item item)
        {
            _narrativeItems.Add(item);
        }
        #endregion

    }
}

