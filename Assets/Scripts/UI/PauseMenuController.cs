using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact me directly
/// </summary>
namespace Mattordev
{
    public class PauseMenuController : MonoBehaviour
    {
        public GameObject pMenu; // ref to the pause menu
        Canvas pMenuCanvas;
        // Start is called before the first frame update
        void Start()
        {
            // Call initialization
            Init();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        /// <summary>
        /// This makes sure everything is assigned at start.
        /// </summary>
        void Init()
        {
            pMenu = this.gameObject;
            // Disable the pause menu at game start.
            pMenuCanvas = pMenu.GetComponent<Canvas>();
            pMenuCanvas.enabled = false;
        }
    }
}
