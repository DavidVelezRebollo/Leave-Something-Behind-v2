using LSB.Shared;
using UnityEngine;

namespace LSB.Components.Items {
    public abstract class Item : MonoBehaviour {
        public ItemInfo _item;
        public virtual void UseItem() {
            
        }
        public virtual void DeleteItem() 
        { 
            //TODO Implent DeleteItem
        }
        public virtual void AddToBackPack() 
        { 
            
        }
    }
}

