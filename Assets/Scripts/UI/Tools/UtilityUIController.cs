using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mattordev.Utils.Stats;
using Mattordev.Utils;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.UI
{
    public class UtilityUIController : MonoBehaviour
    {
        [Header("General")]
        public KeyCode keyToActivate; // The key that activates the Utility window.

        [Header("Canvas Elements")]
        public Canvas utilCanvas; // The Utility canvas obj.
        public Canvas statusWindow; // The status window, shown when the utility canvas is open
        public GameObject utilWindow;
        public GameObject additionWindow;
        public GameObject editWindow;

        [Header("Scripts")]
        public StatisticsTracker statisticsTracker;
        public AddObject addObject;
        public DeleteObject deleteObject;
        public MoveObject moveObject;
        public EditObject editObject;

        /// <summary>
        /// Sets up the canvas for first use
        /// </summary>
        void Start()
        {
            // Get ref to canvas and disable it at game start
            utilCanvas = GetComponent<Canvas>();
            utilCanvas.enabled = false;
            statusWindow.enabled = false;

            // Make sure all of the scripts are in the correct state, aka off.
            statisticsTracker.enabled = false;
            addObject.enabled = false;
            deleteObject.enabled = false;
            moveObject.enabled = false;
        }

        /// <summary>
        /// Call the input check continuely
        /// </summary>
        void Update()
        {
            CheckForInput(keyToActivate);
        }

        /// <summary>
        /// Check for the input, and toggle the UI
        /// </summary>
        /// <param name="key">The key that will toggle the UI</param>
        void CheckForInput(KeyCode key)
        {
            if (Input.GetKeyDown(key))
            {
                Toggle();
                ResetUI();
            }
        }


        /// <summary>
        /// This toggles multiple components when the menu is toggled on and off.
        /// </summary>
        void Toggle()
        {
            utilCanvas.enabled = !utilCanvas.enabled;
            statusWindow.enabled = !statusWindow.enabled;

            // Toggle Scripts
            statisticsTracker.enabled = !statisticsTracker.enabled;
            addObject.enabled = !addObject.enabled;
            deleteObject.enabled = !deleteObject.enabled;
            moveObject.enabled = !moveObject.enabled;
        }

        /// <summary>
        /// Resets the UI to its default state.
        /// </summary>
        private void ResetUI()
        {
            utilWindow.SetActive(true);
            additionWindow.SetActive(false);
            editWindow.SetActive(false);
        }
    }
}
