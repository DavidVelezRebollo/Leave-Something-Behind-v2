using UnityEngine;

namespace LSB.Shared {
    [CreateAssetMenu(fileName = "ItemInfo", menuName = "Game/ItemInfo")]
    public class ItemInfo : ScriptableObject {
        [Tooltip("Sprite of the item")]
        public Sprite Sprite;
        [Tooltip("Spanish name of the item")]
        public string Name;
        [Tooltip("English name of the item")]
        public string EnglishName;
        [Tooltip("Spanish description of what the item does")]
        [Multiline]
        public string Description;
        [Tooltip("English description of what the item does")]
        [Multiline]
        public string EnglishDescription;
        [Tooltip("More accurate description of what the item does in Spanish")]
        [Multiline]
        public string TechnicDescription;
        [Tooltip("More accurate description of what the item does in English")]
        [Multiline]
        public string EnglishTechnicDescription;
        [Tooltip("Type of the item. Normal is a item that gives the player a buff. Narrative can give the player an disadvantage")]
        public ItemType Type;
    }
}
