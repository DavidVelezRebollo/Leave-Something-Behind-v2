using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace LSB.Components.Items
{
    public class Glasses : Item
    {
        private PostProcessVolume _postProcessVolume;
        private void Start()
        {
            _postProcessVolume = Camera.main.GetComponent<PostProcessVolume>();
            AddToBackPack();
        }
        public override void UseItem()
        {
            //nothing
        }

        public override void DeleteItem()
        {
            _postProcessVolume.weight = 1;
        }

        public override void AddToBackPack()
        {
            BackPack.Instance.AddNarrativeItem(this);
        }
    }
}

