using System.Collections;
using UnityEngine;
using LSB.Components.Audio;
using LSB.Shared;
using UnityEngine.Localization.Settings;

namespace LSB.Components.Core {
	public class GameManager : MonoBehaviour {
		#region Singleton
		private GameState _gameState;
		public static GameManager Instance;

		private void Awake() {
			if (Instance != null) return;

			Instance = this;

			_gameState = GameState.Menu;
			
			if(!PlayerPrefs.HasKey("FullScreen")) Screen.SetResolution(Screen.width, Screen.height, true);
			else Screen.SetResolution(Screen.width, Screen.width, PlayerPrefs.GetInt("FullScreen") != 0);
			
			StartCoroutine(initializeLanguage());
		}

		#endregion

		
		[SerializeField] private Texture2D HandleCursor;
		[SerializeField] private Texture2D TargetCursor;

		private float _corruptionDamage;
		private SoundManager _soundManager;
		private Language _language;

		private void Start() {
			_soundManager = SoundManager.Instance;
			_corruptionDamage = 2f;
		}

		public GameState GetGameState()
		{
			return _gameState;
		}

		public Language GetCurrentLanguage() {
			return _language;
		}
		
		public void AddCorruptionDamage(float newDamage)
		{
			_corruptionDamage += newDamage;
		}

		public float GetCorruptionDamage()
		{
			return _corruptionDamage;
		}

		public void SetGameState(GameState state) {
			_gameState = state;
			if (state == GameState.Paused || state == GameState.Menu) {
				Cursor.SetCursor(HandleCursor, new Vector2(0, 0), CursorMode.Auto);
				
				if(state == GameState.Paused) _soundManager.Pause("ThemeSong");
				else _soundManager.Stop("ThemeSong");

				if (state == GameState.Menu) { 
					_corruptionDamage = 2f;
					_soundManager.SetMusicActive(true);
					_soundManager.SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume"));
				}

                _soundManager.Play("MenuSong");
			}
			else if (state == GameState.Running) {
				Cursor.SetCursor(TargetCursor, new Vector2(0, 0), CursorMode.Auto);
				
				if(!_soundManager.IsPlaying("ThemeSong")) _soundManager.Play("ThemeSong");
				else _soundManager.Resume("ThemeSong");
				
				_soundManager.Stop("MenuSong");
			}
		}

		public void SetLanguage(Language language) {
			_language = language;
        }

		private IEnumerator initializeLanguage() {
			yield return LocalizationSettings.InitializationOperation;
			LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[
				PlayerPrefs.HasKey("LocalKey") ? PlayerPrefs.GetInt("LocalKey") : 0];
			_language = PlayerPrefs.HasKey("LocalKey") ? (Language) PlayerPrefs.GetInt("LocalKey") : Language.Spanish;
		}

		public bool GamePaused() { return _gameState != GameState.Running; }
	}
}
