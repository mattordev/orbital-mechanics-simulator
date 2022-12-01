using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.UI
{
    public class PauseMenuController : MonoBehaviour
    {
        public GameObject pMenu; // ref to the pause menu
        Canvas pMenuCanvas;
        public GameObject settingsMenu;

        // Internal
        int pTimeScale = 0;
        float unpausedTimeScale;

        public Animator animator;
        // Start is called before the first frame update
        void Start()
        {
            // Call initialization
            Init();
        }

        // Update is called once per frame
        void Update()
        {
            GetPauseInput();
        }

        /// <summary>
        /// This makes sure everything is assigned at start.
        /// </summary>
        void Init()
        {
            pMenu = this.gameObject;
            animator = GetComponent<Animator>();
            // Disable the pause menu at game start.
            pMenuCanvas = pMenu.GetComponent<Canvas>();
            pMenuCanvas.enabled = false;
            // Get the unpaused timescale
            unpausedTimeScale = Time.timeScale;
        }

        /// <summary>
        /// Pauses the game by setting the timescale
        /// </summary>
        public void Pause(bool paused)
        {
            if (!paused)
            {
                Time.timeScale = unpausedTimeScale;
                animator.SetTrigger("fadeOutPauseMenu");
                return;
            }
            Time.timeScale = pTimeScale;
            animator.SetTrigger("fadeInPauseMenu");
        }

        void GetPauseInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Pause the game
                Pause(!pMenuCanvas.enabled);
                pMenuCanvas.enabled = !pMenuCanvas.enabled;
            }
        }

        public void QuitToMainMenu()
        {
            Time.timeScale = unpausedTimeScale;
            SceneManager.LoadScene(0);
        }

        public void Settings()
        {
            // Play animation for opening settings
            if (!settingsMenu.activeInHierarchy)
            {
                //animator.SetTrigger("fadeInSettings");
            }
            //animator.SetTrigger("fadeOutPauseMenu");
        }
    }
}
