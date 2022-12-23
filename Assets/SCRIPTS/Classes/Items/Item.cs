using UnityEngine;

namespace LSB.Classes.Items {
    public abstract class Item : MonoBehaviour {
        private ItemInfo _item;

        public virtual void UseItem() {
            
        }
        public void DeleteItem() 
        { 
            //TODO Implent DeleteItem
        }
        public void AddToBackPack() 
        { 
            //TODO Implement AddToBackPack
        }
    }
}

