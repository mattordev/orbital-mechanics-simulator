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
        public Canvas statusWindow;

        [Header("Canvas Elements")]
        public GameObject utilWindow;
        public GameObject additionWindow;
        public GameObject editWindow;

        [Header("Scripts")]
        public StatisticsTracker statisticsTracker;
        public AddObject addObject;
        public DeleteObject deleteObject;
        public MoveObject moveObject;
        public EditObject editObject;

        // Start is called before the first frame update
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
            statusWindow.enabled = !statusWindow.enabled;

            // Toggle Scripts
            statisticsTracker.enabled = !statisticsTracker.enabled;
            addObject.enabled = !addObject.enabled;
            deleteObject.enabled = !deleteObject.enabled;
            moveObject.enabled = !moveObject.enabled;


            // statusController.enabled = !statusController.enabled;
        }

        private void ResetUI()
        {
            utilWindow.SetActive(true);
            additionWindow.SetActive(false);
            editWindow.SetActive(false);
        }
    }
}
