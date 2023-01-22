using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LSB.Components.Audio;
using LSB.Components.Core;

namespace LSB.Components.Menus
{
    public class MainMenu : MonoBehaviour
    {
        #region Methods

        
        /// <summary>
        /// Function that runs when the start button is clicked.
        /// </summary>
        public void OnStartButton() {
            GameManager.Instance.SetGameState(GameState.Running);
            SceneManager.LoadScene(1);
            PlayButton();
        }

        /// <summary>
        /// Function that runs when the exit button is clicked.
        /// </summary>
        public void OnExitButton() {
            Application.Quit();
            PlayButton();
        }

        /// <summary>
        /// Function that plays the general sound of a button.
        /// </summary>
        public void PlayButton() {
            SoundManager.Instance.Play("Button");
        }

        #endregion
    }
}
