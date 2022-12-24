using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSB.Shared
{
    [CreateAssetMenu(fileName = "ItemInfo", menuName = "Game/ItemInfo")]
    public class ItemInfo : ScriptableObject
    {
        public Sprite Sprite;
        public string Name;
        public string Description;
        public ItemType Type;
    }
}
