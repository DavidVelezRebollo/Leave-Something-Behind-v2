using UnityEngine;

namespace LSB.Shared {
    [CreateAssetMenu(fileName = "ItemInfo", menuName = "Game/ItemInfo")]
    public class ItemInfo : ScriptableObject {
        [Tooltip("Sprite of the item")]
        public Sprite Sprite;
        [Tooltip("Name of the item")]
        public string Name;
        [Tooltip("Description of what the item does")]
        [Multiline]
        public string Description;
        [Tooltip("Type of the item. Normal is a item that gives the player a buff. Narrative can give the player an disadvantage")]
        public ItemType Type;
    }
}
