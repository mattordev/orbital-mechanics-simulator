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
    public class StatusController : MonoBehaviour
    {
        private static string _statusMessage;
        public static string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                // Check the message len
                if (value.Length > 50)
                {
                    // Warn that the status message might be too long
                    Debug.LogWarning("Status message was too long! It might not fit in the window!");
                    // Set the status variable
                    _statusMessage = value;
                }
                else
                {
                    // Set the status variable
                    _statusMessage = value;
                }
            }
        }

        public TMP_Text TextObject;

        private void Start()
        {
            Clear();
            StatusMessage = "Simulating...";
        }

        // Update is called once per frame
        void Update()
        {
            TextObject.text = StatusMessage;
        }

        public void Clear()
        {
            Debug.Log("Clearing the Status Message");
            TextObject.text = string.Empty;
        }

        public void Clear(int timeToClear)
        {
            StartCoroutine(WaitForSeconds(timeToClear));
        }

        public void SendStatusMessage(string message)
        {
            StatusMessage = message.ToString();
        }

        IEnumerator WaitForSeconds(int timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);
            Clear();
        }
    }
}
