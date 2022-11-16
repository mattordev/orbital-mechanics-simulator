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
        public float xMovementSensitivity = 100f;
        public float yMovementSensitivity = 100f;
        public float amountOfSmoothing;
        private Vector2 posVelocity; 
        private bool focusing;       

        // Background Variables
        [Header("BG Variables")]
        public bool scaleBackground = true;
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
            FocusOnObject();
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
            if(scaleBackground)
                ScaleBackground();
        }

        /// <summary>
        /// Scales the background based on the cameras size.
        /// This needs some fixing, as there's some strange behaviour when fully zoomed
        /// </summary>
        private void ScaleBackground()
        {
            float calculatedScale = currentScale / 5;
            background.transform.localScale = new Vector3(calculatedScale, calculatedScale, calculatedScale) ;
        }

        /// <summary>
        /// Allows the use of camera movement with keyboard input.
        /// </summary>
        public void MoveCameraWithKeyboardInput()
        {
            float x = Input.GetAxis("Horizontal") * Time.deltaTime * xMovementSensitivity;
            float y = Input.GetAxis("Vertical") * Time.deltaTime * yMovementSensitivity;

            Vector2 move = new Vector2(x, y);

            mainCamera.transform.position += new Vector3(move.x, move.y);
        }

        private void FocusOnObject()
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (Input.GetButtonDown("Fire1"))
            {
                // if(hit.transform.tag == "Planet")
                // {
                //Debug.Log("Player is focusing on " + hit.transform.name);
                MoveToClickedTarget(hit.transform);
                    
                // }
            }
        }

        private void MoveToClickedTarget(Transform target)
        {
            if (target == null) {
                transform.parent = null;
                return;
            }
            //Vector2 newCameraPos = Vector2.SmoothDamp(transform.position, pos, ref posVelocity, amountOfSmoothing);
            transform.position = new Vector3(target.position.x, target.position.y, -10);
            transform.parent = target;
        }
    }
}
