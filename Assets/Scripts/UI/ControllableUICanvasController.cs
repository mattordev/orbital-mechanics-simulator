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
        public GameObject velocityVectorPointer;
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

            SpriteRenderer spriteRenderer = velocityVectorPointer.GetComponentInChildren<SpriteRenderer>();

            // Set the velocity vector pointer to the direction of the velocity
            velocityVectorPointer.transform.rotation = Quaternion.Euler(0, 0, cameraController.GetAngleOfVelocity());

            // Adjust the alpha value based on the speed
            Color color = spriteRenderer.color;
            color.a = Mathf.Clamp01(speed);
            spriteRenderer.color = color;
        }

        IEnumerator FadeOutPointer(GameObject pointer, float duration)
        {
            SpriteRenderer spriteRenderer = pointer.GetComponentInChildren<SpriteRenderer>();
            Color color = spriteRenderer.color;
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                // Update the alpha value
                color.a = 1 - (t / duration);
                spriteRenderer.color = color;
                yield return null; // Wait for the next frame
            }
            // Ensure the alpha is set to the target value at the end
            color.a = 0;
            spriteRenderer.color = color;
        }
    }
}
