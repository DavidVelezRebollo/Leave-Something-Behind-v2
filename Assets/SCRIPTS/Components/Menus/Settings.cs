using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using LSB.Components.Audio;
using LSB.Components.Core;
using LSB.Shared;

namespace LSB.Components.Menus {
    public class Settings : MonoBehaviour
    {
        #region Private Fields
        
        [Header("Unity Fields")]
        [Tooltip("AudioMixer.")]
        [SerializeField] private AudioMixer AudioMixer;
        
        [Space(5)]
        [Header("Sliders")]
        [Tooltip("Slider to change the general volume.")]
        [SerializeField] private Slider GeneralSlider;
        [Tooltip("Slider to change the music volume.")]
        [SerializeField] private Slider MusicSlider;
        [Tooltip("Slider to change the sound effects volume.")]
        [SerializeField] private Slider EffectsSlider;

        [Space(5)]
        [Header("Text Fields")]
        [Tooltip("Text that shows the general volume value.")]
        [SerializeField] private TextMeshProUGUI GeneralVolumeText;
        [Tooltip("Text that shows the music volume value.")]
        [SerializeField] private TextMeshProUGUI MusicText;
        [Tooltip("Text that shows the sound effects volume value.")]
        [SerializeField] private TextMeshProUGUI SoundEffectsText;

        [Space(5)] [Header("Dropdowns")] 
        [Tooltip("Dropdown of the languages")] 
        [SerializeField] private TMP_Dropdown LanguageDropdown;

        [Space(5)]
        [Header("Toggles")]
        [Tooltip("Toggle which represents if the tutorial will show or not")]
        [SerializeField] private Toggle TutorialToggle;
        [Tooltip("Toggle which represents if the game is full screen or not")]
        [SerializeField] private Toggle FullScreenToggle;

        private bool _languageActive = false;

        #endregion

        #region Unity Methods

        /// <summary>
        /// When started, it calls for StartSounds() to initialize the values.
        /// </summary>
        private void Start() {
            StartSounds();
        }

        private void OnEnable() {
            initializeToggles();
            initializeDropdowns();
        }

        private void Update() {
            UpdateText();
        }

        #endregion

        #region Methods

        /// <summary>
        /// It initializes all the volumes, the sliders and the texts.
        /// To a preset value if the haven't been changed previously.
        /// Or to the saved values if they have been changed.
        /// </summary>
        private void StartSounds() {
            AudioMixer.SetFloat("Volume", PlayerPrefs.GetFloat("GeneralVolume"));
            
            if (SoundManager.Instance.getMusicActive()) 
                AudioMixer.SetFloat("Music", PlayerPrefs.GetFloat("MusicVolume"));
            else 
                AudioMixer.SetFloat("Music", 0); 
            
            AudioMixer.SetFloat("SoundEffects", PlayerPrefs.GetFloat("SoundEffectsVolume"));
      
            GeneralSlider.value = PlayerPrefs.GetFloat("GeneralVolume");
            MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            EffectsSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume");

            UpdateText();
        }

        /// <summary>
        /// Updates the texts so the reflect the value of the volumes.
        /// </summary>
        private void UpdateText() {
            GeneralVolumeText.text = Mathf.FloorToInt(GetRange(GeneralSlider.maxValue, GeneralSlider.minValue, GeneralSlider.value)).ToString();
            MusicText.text = Mathf.FloorToInt(GetRange(MusicSlider.maxValue, MusicSlider.minValue, MusicSlider.value)).ToString();
            SoundEffectsText.text = Mathf.FloorToInt(GetRange(EffectsSlider.maxValue, EffectsSlider.minValue, EffectsSlider.value)).ToString();
        }
        
        /// <summary>
        /// Initializes the toggles state
        /// </summary>
        private void initializeToggles() {
            if (TutorialToggle != null) {
                if (PlayerPrefs.HasKey("Tutorial"))
                    TutorialToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt("Tutorial") != 0);
                else TutorialToggle.SetIsOnWithoutNotify(true);
            }

            if(PlayerPrefs.HasKey("FullScreen")) 
                FullScreenToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt("FullScreen") != 0);
            else FullScreenToggle.SetIsOnWithoutNotify(true);
        }

        private void initializeDropdowns() {
            LanguageDropdown.value = GameManager.Instance.GetCurrentLanguage() == Language.Spanish ? 0 : 1;
        }
        
        #endregion
        
        #region Getters & Setters

        private float GetRange(float max, float min, float value) {
            return Mathf.Abs(value - min) / (max - min) * 100;
        }

        /// <summary>
        /// Changes the screen mode.
        /// </summary>
        /// <param name="fullScreen">Wether the screen is on full screen mode.</param>
        public void SetFullScreen(bool fullScreen) {
            Screen.SetResolution(Screen.width, Screen.height, fullScreen);
            PlayerPrefs.SetInt("FullScreen", !fullScreen ? 0 : 1);
            SoundManager.Instance.Play("Button");
        }

        public void SetTutorial(bool tutorial) {
            PlayerPrefs.SetInt("Tutorial", !tutorial ? 0 : 1);
            SoundManager.Instance.Play("Button");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume">New value of the general volume.</param>
        public void SetVolume(float volume) {
            AudioMixer.SetFloat("Volume", volume);
            PlayerPrefs.SetFloat("GeneralVolume", volume);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume">New value of the music volume.</param>
        public void SetMusicVolume(float volume) {
            if (!SoundManager.Instance.getMusicActive()) return;
            
            AudioMixer.SetFloat("Music", volume);
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume">New value of the sound effects volume.</param>
        public void SetSoundEffectsVolume(float volume) {
            AudioMixer.SetFloat("SoundEffects", volume);
            PlayerPrefs.SetFloat("SoundEffectsVolume", volume);
        }

        public void ChangeLocale(int localeId) {
            if (_languageActive) return;
            
            StartCoroutine(setLocale(localeId));
        }

        #endregion

        #region Auxiliar Methods

        private IEnumerator setLocale(int localeId) {
            _languageActive = true;
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
            PlayerPrefs.SetInt("LocalKey", localeId);
            GameManager.Instance.SetLanguage(localeId == 0 ? Language.Spanish : Language.English);
            _languageActive = false;

        }

        #endregion
    }
    
}
