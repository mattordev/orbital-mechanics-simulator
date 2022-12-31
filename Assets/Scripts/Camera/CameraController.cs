using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mattordev.Utils.Stats;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
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

        // Background Variables
        [Header("BG Variables")]
        public bool scaleBackground = true;
        public GameObject background;

        [Header("Focus Variables")]
        public float amountOfSmoothing;
        public GameObject currentlyFocusedOn;
        private Vector2 smoothedCameraPos;
        private Vector2 posVelocity;
        public bool focusing;

        // Other Variables
        [SerializeField] private AddObject addObject;
        [SerializeField] private MoveObject moveObject;
        [SerializeField] private StatisticsTracker statisticsTracker;

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

            // Focusing
            if (!moveObject.moving)
            {

                UpdateSmoothDampPos();
                FocusOnObject();
            }
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
            if (scaleBackground)
                ScaleBackground();
        }

        /// <summary>
        /// Scales the background based on the cameras size.
        /// This needs some fixing, as there's some strange behaviour when fully zoomed
        /// </summary>
        private void ScaleBackground()
        {
            float calculatedScale = currentScale / 5;
            background.transform.localScale = new Vector3(calculatedScale, calculatedScale, calculatedScale);
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
            RaycastHit2D hit = Physics2D.Raycast(GetMousePos(), Vector2.zero);

            if (Input.GetButtonDown("Fire1") && !addObject.placing)
            {
                MoveToClickedTarget(hit.transform);
                // string selectedBodyName = statisticsTracker.GetSelectedBody().name;
                // (float orbitalPeriod, float apoapsis, float periapsis) = statisticsTracker.orbitalParameters[selectedBodyName];
            }
        }

        public GameObject GetClickedItem()
        {
            RaycastHit2D hit = Physics2D.Raycast(GetMousePos(), Vector2.zero);

            if (hit.collider == null)
            {
                Debug.LogError("Didn't hit a collider!");
                return null;
            }
            return hit.transform.gameObject;
        }

        public void MoveToClickedTarget(Transform target)
        {
            if (target == null)
            {
                transform.parent = null;
                currentlyFocusedOn = null;
                focusing = false;
                return;
            }
            // Tell the rest of the script that a planet has been focused
            focusing = true;
            // Set the inspector variable.
            currentlyFocusedOn = target.gameObject;
            //transform.position = new Vector3(target.position.x, target.position.y, -10);

            transform.parent = target;
        }

        void UpdateSmoothDampPos()
        {
            // If we're not focusing, return out of the function.
            if (!focusing)
            {
                return;
            }
            // Create the smooth camera pos based on the cameras current location and the focused target.
            smoothedCameraPos = Vector2.SmoothDamp(transform.position, currentlyFocusedOn.transform.position, ref posVelocity, amountOfSmoothing);
            // Move the camera to the new target, whilst keeping the z value intact
            transform.position = new Vector3(smoothedCameraPos.x, smoothedCameraPos.y, -10);
        }

        public Vector2 GetMousePos()
        {
            return mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
