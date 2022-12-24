using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using LSB.Components.Audio;

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
        AudioMixer.SetFloat("Music", PlayerPrefs.GetFloat("MusicVolume"));
        AudioMixer.SetFloat("SoundEffects", PlayerPrefs.GetFloat("SoundEffectsVolume"));

        GeneralSlider.value = PlayerPrefs.GetFloat("GeneralVolume") / (_soundRange.y + _soundRange.x - _soundRange.y);
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume") / (_soundRange.y + _soundRange.x - _soundRange.y);
        EffectsSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume") / (_soundRange.y + _soundRange.x - _soundRange.y);

        UpdateText();
    }

    /// <summary>
    /// Updates the texts so the reflect the value of the volumes.
    /// </summary>
    protected void UpdateText()
    {
        GeneralVolumeText.text = Mathf.FloorToInt((GeneralSlider.value * 100)).ToString();
        MusicText.text = Mathf.FloorToInt((MusicSlider.value * 100)).ToString();
        SoundEffectsText.text = Mathf.FloorToInt((EffectsSlider.value * 100)).ToString();
    }

    /// <summary>
    /// Changes the screen mode.
    /// </summary>
    /// <param name="fullScreen">Wether the screen is on full screen mode.</param>
    protected void SetFullScreen(bool fullScreen)
    {
        Screen.SetResolution(Screen.width, Screen.height, fullScreen);
        SoundManager.Instance.Play("Button");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="volume">New value of the general volume.</param>
    protected void SetVolume(float volume)
    {
        AudioMixer.SetFloat("Volume", (volume * (_soundRange.x - _soundRange.y)) + _soundRange.y);
        PlayerPrefs.SetFloat("GeneralVolume", (volume * (_soundRange.x - _soundRange.y)) + _soundRange.y);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="volume">New value of the music volume.</param>
    protected void SetMusicVolume(float volume)
    {
        AudioMixer.SetFloat("Music", (volume * (_soundRange.x - _soundRange.y)) + _soundRange.y);
        PlayerPrefs.SetFloat("MusicVolume", (volume * (_soundRange.x - _soundRange.y)) + _soundRange.y);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="volume">New value of the sound effects volume.</param>
    protected void SetSoundEffectsVolume(float volume)
    {
        AudioMixer.SetFloat("SoundEffects", (volume * (_soundRange.x - _soundRange.y)) + _soundRange.y);
        PlayerPrefs.SetFloat("SoundEffectsVolume", (volume * (_soundRange.x - _soundRange.y)) + _soundRange.y);
    }


    #endregion
}
