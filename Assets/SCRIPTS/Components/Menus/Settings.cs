using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using LSB.Components.Audio;

namespace LSB.Components.Menus {
    public class Settings : MonoBehaviour
    {
        #region PublicVariables

        [Tooltip("AudioMixer.")]
        public AudioMixer AudioMixer;

        private Vector2 _soundRange = new Vector2(10, -20);

        [Tooltip("Slider to change the general volume.")]
        public Slider GeneralSlider;
        [Tooltip("Slider to change the music volume.")]
        public Slider MusicSlider;
        [Tooltip("Slider to change the sound effects volume.")]
        public Slider EffectsSlider;

        #endregion

        #region _privateVariables

        [Tooltip("Text that shows the general volume value.")]
        [SerializeField] private TextMeshProUGUI GeneralVolumeText;
        [Tooltip("Text that shows the music volume value.")]
        [SerializeField] private TextMeshProUGUI MusicText;
        [Tooltip("Text that shows the sound effects volume value.")]
        [SerializeField] private TextMeshProUGUI SoundEffectsText;

        #endregion

        #region Unity Methods

        /// <summary>
        /// When started, it calls for StartSounds() to initialize the values.
        /// </summary>
        private void Start()
        {
            GeneralSlider.maxValue = _soundRange.x;
            GeneralSlider.minValue = _soundRange.y;
            MusicSlider.maxValue = _soundRange.x;
            MusicSlider.minValue = _soundRange.y;
            EffectsSlider.maxValue = _soundRange.x;
            EffectsSlider.minValue = _soundRange.y;
            StartSounds();
        }

        private void Update()
        {
            UpdateText();
        }

        #endregion

        #region Methods

        /// <summary>
        /// It initializes all the volumes, the sliders and the texts.
        /// To a preset value if the haven't been changed previously.
        /// Or to the saved values if they have been changed.
        /// </summary>
        public void StartSounds()
        {
            AudioMixer.SetFloat("Volume", PlayerPrefs.GetFloat("GeneralVolume"));
            if (SoundManager.Instance.getMusicActive()) { AudioMixer.SetFloat("Music", PlayerPrefs.GetFloat("MusicVolume")); }
            else { AudioMixer.SetFloat("Music", 0); }
            AudioMixer.SetFloat("SoundEffects", PlayerPrefs.GetFloat("SoundEffectsVolume"));


            GeneralSlider.value = PlayerPrefs.GetFloat("GeneralVolume");
            MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            EffectsSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume");

            UpdateText();
        }

        /// <summary>
        /// Updates the texts so the reflect the value of the volumes.
        /// </summary>
        protected void UpdateText()
        {
            GeneralVolumeText.text = Mathf.FloorToInt(GetRange(GeneralSlider.maxValue, GeneralSlider.minValue, GeneralSlider.value)).ToString();
            MusicText.text = Mathf.FloorToInt(GetRange(MusicSlider.maxValue, MusicSlider.minValue, MusicSlider.value)).ToString();
            SoundEffectsText.text = Mathf.FloorToInt(GetRange(EffectsSlider.maxValue, EffectsSlider.minValue, EffectsSlider.value)).ToString();
        }

        private float GetRange(float max, float min, float value)
        {
            return Mathf.Abs(value - min) / (max - min) * 100;
        }

        /// <summary>
        /// Changes the screen mode.
        /// </summary>
        /// <param name="fullScreen">Wether the screen is on full screen mode.</param>
        public void SetFullScreen(bool fullScreen)
        {
            Screen.SetResolution(Screen.width, Screen.height, fullScreen);
            SoundManager.Instance.Play("Button");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume">New value of the general volume.</param>
        public void SetVolume(float volume)
        {
            AudioMixer.SetFloat("Volume", volume);
            PlayerPrefs.SetFloat("GeneralVolume", volume);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume">New value of the music volume.</param>
        public void SetMusicVolume(float volume)
        {
            if (SoundManager.Instance.getMusicActive())
            {
                AudioMixer.SetFloat("Music", volume);
                PlayerPrefs.SetFloat("MusicVolume", volume);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume">New value of the sound effects volume.</param>
        public void SetSoundEffectsVolume(float volume)
        {
            AudioMixer.SetFloat("SoundEffects", volume);
            PlayerPrefs.SetFloat("SoundEffectsVolume", volume);
        }


        #endregion
    }
    
}
