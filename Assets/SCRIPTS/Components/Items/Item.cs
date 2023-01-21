using LSB.Shared;
using UnityEngine;

namespace LSB.Components.Items {
    public abstract class Item : MonoBehaviour {
        [Tooltip("Info of the item")]
        [SerializeField] protected ItemInfo Info;
        
        /// <summary>
        /// Applies the item effects to the player or to the game
        /// </summary>
        public virtual void UseItem() {
            Debug.LogWarning("TO DO - Use item: " + Info.Name);
        }
        
        /// <summary>
        /// Undo the effect of the item
        /// </summary>
        public virtual void UndoItem() { 
            Debug.LogWarning("TO DO - Delete item: " + Info.Name);
        }

        #region Getters

        /// <summary>
        /// Gets the sprite of the item
        /// </summary>
        /// <returns>The sprite of the item</returns>
        public Sprite GetSprite() {
            return Info.Sprite;
        }

        /// <summary>
        /// Gets the name of the item
        /// </summary>
        /// <returns>The name of the item</returns>
        public string GetName() {
            return Info.Name;
        }

        public string GetTechnicalDescription() {
            return Info.TechnicDescription;
        }

        /// <summary>
        /// Gets the description of the item
        /// </summary>
        /// <returns>The description of the item</returns>
        public string GetDescription() {
            return Info.Description;
        }

        #endregion
        
        
    }
}

