using System.Collections;
using System.Collections.Generic;
using LSB.Components.Audio;
using LSB.Components.Core;
using LSB.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LSB.Components.UI {
    public class Tutorial : MonoBehaviour {
        [SerializeField] private GameObject NextTutorial;
        [SerializeField] private TextMeshProUGUI[] TextToDisplay;
        [SerializeField] [TextArea(3, 10)] private string[] SpanishTextToWrite;
        [SerializeField] [TextArea(3, 10)] private string[] EnglishTextToWrite;
        [SerializeField] private Image[] Images;

        private Queue<string> _texts;
        private bool _finished;
        private SoundManager _soundManager;
        private GameManager _gameManager;

        private void OnEnable() {
            _soundManager = SoundManager.Instance;
            _gameManager = GameManager.Instance;
            _texts = new Queue<string>();

            if (!PlayerPrefs.HasKey("Tutorial") || PlayerPrefs.GetInt("Tutorial") == 1) {
                StartCoroutine(displayTutorial());
                if (Images.Length > 0) StartCoroutine(displayImages());
            }
            else {
                foreach (TextMeshProUGUI textContainer in TextToDisplay)
                    foreach (string text in SpanishTextToWrite) {
                        textContainer.text = text;
                    }
            }
        }

        private IEnumerator displayTutorial() {
            _texts.Clear();

            foreach (string text in _gameManager.GetCurrentLanguage() == Language.Spanish ? SpanishTextToWrite : EnglishTextToWrite) {
                _texts.Enqueue(text);
            }

            foreach (TextMeshProUGUI container in TextToDisplay) {
                _finished = false;
                StartCoroutine(typeSentence(container, _texts.Peek()));

                while (!_finished) {
                    yield return null;
                }

                _texts.Dequeue();
                yield return new WaitForSeconds(0.5f);
            }
        }

        private IEnumerator displayImages() {
            foreach (Image image in Images) {
                _soundManager.Play("TutorialImage");
                for (float i = 0; image.fillAmount < 1; i += 0.5f * Images.Length * Time.deltaTime) {
                    image.fillAmount += i;
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }

        private IEnumerator typeSentence(TextMeshProUGUI container, string text) {
            container.text = "";

            foreach (char c in text) {
                container.text += c;
                _soundManager.Play(Random.Range(0f, 1f) < 0.5f ? "Keypress" : "Keypress2");
                yield return new WaitForSeconds(0.07f);
            }

            _finished = true;
        }

        public void OnContinueButton() {
            if (_texts.Count > 0) {
                StopAllCoroutines();
                int i = 0;

                foreach (TextMeshProUGUI container in TextToDisplay) {
                    container.text = _gameManager.GetCurrentLanguage() == Language.Spanish ? SpanishTextToWrite[i] : EnglishTextToWrite[i];
                    i++;
                }
                
                _texts.Clear();

                if (Images.Length <= 0) return;
                
                foreach (Image image in Images) {
                    image.fillAmount = 1f;
                }

                return;
            }

            if (NextTutorial != null) {
                NextTutorial.SetActive(true);
                gameObject.SetActive(false);
                _soundManager.Play("Button");
            }
            else {
                transform.parent.gameObject.SetActive(false);
                _gameManager.SetGameState(GameState.Running);
            }
        }
    }
}
