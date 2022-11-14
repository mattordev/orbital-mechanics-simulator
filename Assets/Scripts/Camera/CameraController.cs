using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact me directly
/// </summary>
namespace Mattordev.Utils
{
    public class CameraController : MonoBehaviour
    {
        public Camera mainCamera;

        // Camera zooming scale Variables
        [Header("Zooming Variables")]
        public int maxScale = 100;
        public int minScale = 10;
        public int zoomSensitivity = 10;
        private float currentScale;

        [Header("Camera Movement Variables")]
        public float xMovementSensitivity = 5f;
        public float yMovementSensitivity = 5f;

        // Background Variables
        [Header("BG Variables")]
        public GameObject background;

        // Start is called before the first frame update
        void Start()
        {
            // Set the camera object to whatever the main camera is.
            mainCamera = Camera.main;   
        }

        // Update is called once per frame
        void Update()
        {
            MoveCameraWithKeyboardInput();
            ZoomScale();
        }

        /// <summary>
        /// Allows the player to scroll to zoom into things.
        /// </summary>
        public void ZoomScale()
        {
            currentScale = mainCamera.orthographicSize;
            currentScale -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
            currentScale = Mathf.Clamp(currentScale, minScale, maxScale);
            mainCamera.orthographicSize = currentScale;

            // Scale the background with the screensize
            ScaleBackground();
        }

        /// <summary>
        /// Scales the background based on the cameras size.
        /// </summary>
        private void ScaleBackground()
        {
            float calculatedScale = currentScale / 5;
            background.transform.localScale = new Vector3(calculatedScale, calculatedScale, calculatedScale) ;
        }

        public void MoveCameraWithKeyboardInput()
        {
            float x = Input.GetAxis("Horizontal") * Time.deltaTime * xMovementSensitivity;
            float y = Input.GetAxis("Vertical") * Time.deltaTime * yMovementSensitivity;

            Vector2 move = new Vector2(x, y);

            mainCamera.transform.position += new Vector3(move.x, move.y);
        }
    }
}
