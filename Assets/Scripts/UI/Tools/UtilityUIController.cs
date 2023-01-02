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
        public KeyCode keyToActivate;
        public Canvas utilCanvas;

        [Header("Canvas Elements")]
        public GameObject utilWindow;
        public GameObject additionWindow;

        [Header("Scripts")]
        public StatisticsTracker statisticsTracker;
        public AddObject addObject;
        public DeleteObject deleteObject;
        public MoveObject moveObject;
        public StatusController statusController;

        // Start is called before the first frame update
        void Start()
        {
            // Get ref to canvas and disable it at game start
            utilCanvas = GetComponent<Canvas>();
            utilCanvas.enabled = false;

            // Make sure all of the scripts are in the correct state, aka off.
            statisticsTracker.enabled = false;
            addObject.enabled = false;
            deleteObject.enabled = false;
            moveObject.enabled = false;
            statusController.enabled = !statusController.enabled;

            // StatusController.StatusMessage = "0123456789012345678901234567890123456789";
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


        /// <summary>
        /// This toggles multiple components when the menu is toggled on and off.
        /// </summary>
        void Toggle()
        {
            utilCanvas.enabled = !utilCanvas.enabled;

            // Toggle Scripts
            statisticsTracker.enabled = !statisticsTracker.enabled;
            addObject.enabled = !addObject.enabled;
            deleteObject.enabled = !deleteObject.enabled;
            moveObject.enabled = !moveObject.enabled;
            statusController.enabled = !statusController.enabled;
        }

        private void ResetUI()
        {
            utilWindow.SetActive(true);
            additionWindow.SetActive(false);
        }
    }
}
