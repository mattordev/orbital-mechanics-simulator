using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.UI
{
    /// <summary>
    /// A class to control the status text and handle updating the UI. This tells the user what is going on in the simulation. 
    /// </summary>
    public class StatusController : MonoBehaviour
    {
        private static string _statusMessage;
        public const int MaxStatusMessageLen = 50;
        public static string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                if (value == null)
                {
                    Debug.LogWarning("Status message was null!");
                    return;
                }

                if (value.Length > MaxStatusMessageLen)
                {
                    Debug.LogWarning("Status message was too long! It might not fit in the window.");
                    // Trims the string between 0 chars and the max length.
                    value = value.Substring(0, MaxStatusMessageLen);
                }

                _statusMessage = value;
            }
        }

        public TMP_Text textObject;

        /// <summary>
        /// Start method clears the status message and sets it back to its default message. 
        /// </summary>
        private void Start()
        {
            ClearStatusMessage();
            StatusMessage = "Simulating...";
        }

        /// <summary>
        /// Sets the text on the UI so it matches the status message
        /// </summary>
        void Update()
        {
            textObject.text = StatusMessage;
        }

        /// <summary>
        /// Clears the status message and logs it to the console.
        /// </summary>
        public void ClearStatusMessage()
        {
            Debug.Log("Clearing the Status Message");
            textObject.text = string.Empty;
        }

        /// <summary>
        /// Clears the console after a set time. Implements a coroutine and calls the ClearStatusMessage function.
        /// </summary>
        /// <param name="timeToClear">The time to wait before clearing(in seconds).</param>
        public void StartDelayedClear(int timeToClear)
        {
            StartCoroutine(WaitForSeconds(timeToClear));
        }

        /// <summary>
        /// Coroutine that waits before clearing the message
        /// </summary>
        /// <param name="timeToWait">The time to wait before clearing(in seconds).</param>
        /// <returns></returns>
        IEnumerator WaitForSeconds(int timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);
            ClearStatusMessage();
        }
    }
}
