using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev
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
