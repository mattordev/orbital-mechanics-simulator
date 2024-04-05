using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mattordev.Utils;
using Mattordev.Spaceship;
using Unity.Mathematics;

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
        public TMP_Text inertialDampenersText;
        public TMP_Text nameText;

        // Other
        private CameraController cameraController;
        private SpaceshipController spaceshipController;

        // Start is called before the first frame update
        void Start()
        {
            showCanvas = false;
            cameraController = Camera.main.GetComponent<CameraController>();
            spaceshipController = FindObjectOfType<SpaceshipController>();
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
            speedText.text = speed.ToString("F2");
            inertialDampenersText.text = spaceshipController.inertialDampeners ? "On" : "Off";

            UpdateVelocityVectorPointer(speed);
        }

        public void UpdateVelocityVectorPointer(float speed)
        {
            SpriteRenderer spriteRenderer = velocityVectorPointer.GetComponentInChildren<SpriteRenderer>();

            // Set the velocity vector pointer to the direction of the velocity
            velocityVectorPointer.transform.rotation = Quaternion.Euler(0, 0, cameraController.GetAngleOfVelocity());

            // Adjust the alpha value based on the speed
            Color color = spriteRenderer.color;
            color.a = Mathf.Clamp01(speed);
            spriteRenderer.color = color;
        }
    }
}
