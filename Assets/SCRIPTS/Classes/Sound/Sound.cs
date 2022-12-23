using UnityEngine;

namespace LSB.Classes.Sound {
    public class Sound : MonoBehaviour {
        public string ClipName;
        public AudioType Type;
        public AudioClip Clip;
        public bool IsLoop;
        public bool PlayOnAwake;
        [Range(0, 1)] public float Volume;
        
        [HideInInspector] public AudioSource Source;
    }
}
