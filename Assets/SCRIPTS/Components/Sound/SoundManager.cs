using UnityEngine;
using UnityEngine.Audio;
using System;
using LSB.Shared;

namespace LSB.Components.Audio {
	public class SoundManager : MonoBehaviour {
        #region Instance
        [Tooltip("Instance of SoundManager, so it can be accessed from other classes.")]
        public static SoundManager Instance;
        private bool isMusicActive = true;
        #endregion

        #region _privateVariables

        [Tooltip("Group of general sounds. SerializeField: you can change it from the editor.")]
        [SerializeField] private AudioMixerGroup GeneralMixerGroup;
        [Tooltip("Group of Music type sounds. SerializeField: you can change it from the editor.")]
        [SerializeField] private AudioMixerGroup MusicMixerGroup;
        [Tooltip("Group of SoundEffect type sounds. SerializeField: you can change it from the editor.")]
        [SerializeField] private AudioMixerGroup SoundEffectsMixerGroup;
        [Tooltip("Array of sounds. SerializeField: you can change it from the editor.")]
        [SerializeField] private Classes.Sound.Sound[] Sounds;

        #endregion

        #region Unity Events

        /// <summary>
        /// Initializes Instance. Calls for it to not be destroyed when loading new scenes.
        /// It initializes each sound in _sounds, establishes their type, an plays them if playOnAwake is true.
        /// </summary>
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);


            foreach (Classes.Sound.Sound s in Sounds)
            {
                s.Source = gameObject.AddComponent<AudioSource>();
                s.Source.clip = s.AudioClip;
                s.Source.loop = s.IsLoop;
                s.Source.volume = s.Volume;

                switch (s.AudioType)
                {
                    case SoundType.SoundEffect:
                        s.Source.outputAudioMixerGroup = SoundEffectsMixerGroup;
                        break;

                    case SoundType.Music:
                        s.Source.outputAudioMixerGroup = MusicMixerGroup;
                        break;
                }

                if (s.PlayOnAwake)
                    s.Source.Play();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// It checks if the settings of the mixers have been changed and if so, it assigns it values. 
        /// If not, it initializes the mixers volume.
        /// </summary>
        private void Start()
        {
            
            if (PlayerPrefs.HasKey("GeneralVolume") &&
               PlayerPrefs.HasKey("SoundEffectsVolume") &&
               PlayerPrefs.HasKey("MusicVolume"))
                LoadVolume();
            
            else
            {
                PlayerPrefs.SetFloat("GeneralVolume", 0);
                PlayerPrefs.SetFloat("SoundEffectsVolume", 0);
                PlayerPrefs.SetFloat("MusicVolume", 0);
            }
            
        }

        public void setMusicVolume(float volume)
        {
            MusicMixerGroup.audioMixer.SetFloat("Music", volume);
        }
        public void setMusicActive(bool active)
        {
            isMusicActive = active;
        }
        public bool getMusicActive()
        {
            return isMusicActive;
        }
        /// <summary>
        /// Loads the volume of the mixers.
        /// </summary>
        private void LoadVolume()
        {
            GeneralMixerGroup.audioMixer.SetFloat("Volume", PlayerPrefs.GetFloat("GeneralVolume"));
            SoundEffectsMixerGroup.audioMixer.SetFloat("SoundEffects", PlayerPrefs.GetFloat("SoundEffectsVolume"));

            if (isMusicActive) MusicMixerGroup.audioMixer.SetFloat("Music", PlayerPrefs.GetFloat("MusicVolume")); 
            else { MusicMixerGroup.audioMixer.SetFloat("Music", 0); }
        }

        /// <summary>
        /// Finds the clip named "clipname" and plays it if it exists.
        /// </summary>
        /// <param name="clipName">Name of the clip to Play.</param>
        public void Play(string clipName)
        {
            Classes.Sound.Sound s = Array.Find(Sounds, dummySound => dummySound.ClipName == clipName);
            if (s == null)
            {
                Debug.LogError("Sound " + clipName + " not found");
                return;
            }
            s.Source.Play();
        }

        public void PlayOneShot(string clipName)
        {
            Classes.Sound.Sound s = Array.Find(Sounds, dummySound => dummySound.ClipName == clipName);
            if (s == null)
            {
                Debug.LogError("Sound " + clipName + " not found");
                return;
            }
            s.Source.PlayOneShot(s.AudioClip);
        }

        /// <summary>
        /// Finds the clip named "clipname" and stops it if it exists.
        /// </summary>
        /// <param name="clipName">Name of the clip to Stop.</param>
        public void Stop(string clipName)
        {
            Classes.Sound.Sound s = Array.Find(Sounds, dummySound => dummySound.ClipName == clipName);
            if (s == null)
            {
                Debug.LogError("Sound " + clipName + " not found");
                return;
            }
            s.Source.Stop();
        }


        #endregion
    }
}
