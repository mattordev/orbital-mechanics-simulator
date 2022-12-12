using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.UI
{
    public class UtilityUIController : MonoBehaviour
    {
        public KeyCode keyToActivate;
        public Canvas utilCanvas;

        [Header("Canvas Elements")]
        public GameObject utilWindow;
        public GameObject additionWindow;

        // Start is called before the first frame update
        void Start()
        {
            // Get ref to canvas and disable it at game start
            utilCanvas = GetComponent<Canvas>();
            utilCanvas.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            CheckForInput(keyToActivate);
        }

        void CheckForInput(KeyCode key)
        {
            if (Input.GetKeyDown(key))
            {
                Toggle();
                ResetUI();
            }
        }

        void Toggle()
        {
            utilCanvas.enabled = !utilCanvas.enabled;
        }

        void ResetUI()
        {
            utilWindow.SetActive(true);
            additionWindow.SetActive(false);
        }
    }
}
