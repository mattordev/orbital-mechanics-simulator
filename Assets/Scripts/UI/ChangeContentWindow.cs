using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.UI
{
    public class ChangeContentWindow : MonoBehaviour
    {
        public GameObject[] contentWindows;
        public ChiScrollRect chiScrollRect;

        // Start is called before the first frame update
        void Start()
        {
            // Enable the first content window when the shop is opened.
            contentWindows[0].SetActive(true);
        }

        public void ChangeContent(GameObject button)
        {
            switch (button.GetComponent<ContentIndexer>().buttonIndex)
            {
                case 0:
                    UnityEngine.Debug.Log("Changing content window to 0");
                    contentWindows[0].SetActive(true);
                    chiScrollRect.content = contentWindows[0].GetComponent<RectTransform>();
                    break;
                case 1:
                    UnityEngine.Debug.Log("Changing content window to 1");
                    contentWindows[1].SetActive(true);
                    chiScrollRect.content = contentWindows[1].GetComponent<RectTransform>();
                    break;
                case 2:
                    UnityEngine.Debug.Log("Changing content window to 2");
                    contentWindows[2].SetActive(true);
                    chiScrollRect.content = contentWindows[2].GetComponent<RectTransform>();
                    break;
                default:
                    UnityEngine.Debug.Log("Invalid button index.");
                    break;
            }
        }
    }
}
