using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact me directly
/// </summary>
namespace mattordev
{
    public class MainMenuController : MonoBehaviour
    {
        public void PlayGame(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void Quit()
        {
            Debug.Log("Game is quitting!");
            Application.Quit();
        }
    }
}
