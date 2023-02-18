using System.Collections.Generic;
using System.Collections;
using LSB.Components.Core;
using LSB.Shared;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RoundTableStudio.UI
{
	public class Cinematic : MonoBehaviour
	{
		[Header("Text fields")]
		[Tooltip("Text that will appear")]
		[TextArea(3, 10)] public string[] SpanishSentences;
		[TextArea(3, 10)] public string[] EnglishSentences;
		[Space(10)]
		[Header("Reference fields")]
		public TextMeshProUGUI CinematicText;
		public GameObject LoadingScreen;
		public Image LoadingBar;
		
		private Queue<string> _sentences;
		private bool _finished;
		[SerializeField] private GameObject ContinueButton;

		private void OnEnable() {
			_sentences = new Queue<string>();

			StartCoroutine(StartCinematic());
		}

		private IEnumerator StartCinematic() {
			_sentences.Clear();

			foreach (string sentence in GameManager.Instance.GetCurrentLanguage() == Language.Spanish ? SpanishSentences : EnglishSentences)
			{
				_sentences.Enqueue(sentence);
			}

			foreach (string s in _sentences)
			{

				_finished = false;
				DisplayNextSentence(s);

				while (!_finished)
				{
					yield return null;
				}

				yield return new WaitForSeconds(1.5f);
			}

			//EndCinematic();
			if (SceneManager.GetActiveScene().buildIndex == 0) StartCoroutine(loadSceneAsync());
			else if (ContinueButton!=null) ContinueButton.SetActive(true);
		}

		private void DisplayNextSentence(string sentence) {
			StartCoroutine(TypeSentence(sentence));
		}

		private IEnumerator TypeSentence(string sentence) {
			CinematicText.text = "";

			foreach (char letter in sentence) {
				CinematicText.text += letter;
				yield return new WaitForSeconds(0.07f);
			}

			_finished = true;
		}

		private IEnumerator loadSceneAsync() {
			AsyncOperation loadScene = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
			LoadingScreen.SetActive(true);

			while (!loadScene.isDone) {
				float progressValue = Mathf.Clamp01(loadScene.progress / 0.09f);
				LoadingBar.fillAmount = progressValue;

				yield return null;
			}
		}
	}
}

