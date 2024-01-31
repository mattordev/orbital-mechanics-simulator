using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mattordev.Utils;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.UI
{
    public class ControllableUICanvasController : MonoBehaviour
    {
        public bool showCanvas;

        // Canvas elements
        public Canvas controllableUICanvas;
        public Image shipSprite;
        public Image velocityVectorPointer;
        public TMP_Text speedText;
        public TMP_Text nameText;

        // Other
        private CameraController cameraController;

        // Start is called before the first frame update
        void Start()
        {
            showCanvas = false;
            cameraController = Camera.main.GetComponent<CameraController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (showCanvas)
            {
                controllableUICanvas.enabled = true;
            }
            else
            {
                controllableUICanvas.enabled = false;
            }
        }

        public void SetElements(Sprite controllableSprite, string name, float speed)
        {
            shipSprite.sprite = controllableSprite;
            nameText.text = name;
            speedText.text = speed.ToString();

        }
    }
}
