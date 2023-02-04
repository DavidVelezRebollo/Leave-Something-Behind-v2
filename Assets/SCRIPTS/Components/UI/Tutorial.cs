using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using LSB.Components.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LSB.Components.UI {
    public class Tutorial : MonoBehaviour {
        [SerializeField] private GameObject NextTutorial;
        [SerializeField] private TextMeshProUGUI[] TextToDisplay;
        [SerializeField] [TextArea(3, 10)] private string[] TextToWrite;
        [SerializeField] private Image[] Images;

        private Queue<string> _texts;
        private bool _finished = false;
        private Tween _typeWrite;

        private void OnEnable() {
            _texts = new Queue<string>();
            
            if (!PlayerPrefs.HasKey("Tutorial") || PlayerPrefs.GetInt("Tutorial") == 1) {
                StartCoroutine(displayTutorial());
                if (Images.Length > 0) StartCoroutine(displayImages());
            }
            else {
                foreach (TextMeshProUGUI textContainer in TextToDisplay)
                    foreach (string text in TextToWrite) {
                        textContainer.text = text;
                    }
            }
        }

        private IEnumerator displayTutorial() {
            _texts.Clear();

            foreach (string text in TextToWrite) {
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
                for (float i = 0; image.fillAmount < 1; i += 0.001f * Images.Length) {
                    image.fillAmount += i;
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        private IEnumerator typeSentence(TextMeshProUGUI container, string text) {
            container.text = "";

            foreach (char c in text) {
                container.text += c;
                yield return new WaitForSeconds(0.07f);
            }

            _finished = true;
        }

        public void OnContinueButton() {
            if (_texts.Count > 0) {
                StopAllCoroutines();
                int i = 0;

                foreach (TextMeshProUGUI container in TextToDisplay) {
                    container.text = TextToWrite[i];
                    i++;
                }
                
                _texts.Clear();

                if (Images.Length <= 0) return;
                
                foreach (Image image in Images) {
                    image.fillAmount = 1f;
                }

                return;
            }

            NextTutorial.SetActive(true);
            gameObject.SetActive(false);
            SoundManager.Instance.Play("Button");
        }
    }
}
