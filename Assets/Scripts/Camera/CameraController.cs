using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mattordev.Utils.Stats;
using Mattordev.UI;
using Mattordev.Spaceship;

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

        [Header("Other Controls")]
        public KeyCode activationKey = KeyCode.E;

        // Other Variables
        [SerializeField] private AddObject addObject;
        [SerializeField] private MoveObject moveObject;
        [SerializeField] private EditObject editObject;
        [SerializeField] private StatisticsTracker statisticsTracker;

        SpaceshipController spaceshipController;

        // Start is called before the first frame update
        void Start()
        {
            // Set the camera object to whatever the main camera is.
            mainCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            GetKeyboardInput();
            ZoomScale();

            // Focusing
            // make sure that we're not moving another object
            if (moveObject.moving)
            {
                return;
            }

            if (focusing)
            {
                UpdateSmoothDampPos();
            }

            if (Input.GetButtonDown("Fire1"))
            {
                FocusOnObject();
            }

            if (spaceshipController != null && spaceshipController.controlling)
            {
                UpdateShipUI();
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

        public void GetKeyboardInput()
        {
            if (IsControllableObject() && Input.GetKeyUp(activationKey))
            {
                Debug.Log("controllable object");
                if (currentlyFocusedOn.tag == "Spaceship")
                {
                    spaceshipController = currentlyFocusedOn.GetComponent<SpaceshipController>();
                    // Invert the control check to toggle controls
                    spaceshipController.controlling = !spaceshipController.controlling;
                    StatusController.StatusMessage = "Controlling Spaceship, press \"E\" to stop";
                }
            }

            if (!IsControllableObject())
            {
                MoveCameraWithKeyboardInput();
            }
        }

        /// <summary>
        /// Updates the ship controllable UI to show the correct elements
        /// </summary>
        public void UpdateShipUI()
        {
            // Need to update speed every frame, currently sprite isn't setting right either
            // Listen, I'm not proud of these next few lines....
            ControllableUICanvasController controllableUICanvas = FindObjectOfType<ControllableUICanvasController>();
            SpriteRenderer spriteRenderer = currentlyFocusedOn.GetComponentInChildren<SpriteRenderer>();
            Sprite sprite = spriteRenderer.sprite;
            Rigidbody2D rb = currentlyFocusedOn.GetComponent<Rigidbody2D>();
            // to fix, sprite not setting, speed only updates on control change
            controllableUICanvas.SetElements(sprite, currentlyFocusedOn.gameObject.name, rb.velocity.magnitude);
            // Enable the controllable canvas
            controllableUICanvas.showCanvas = true;
        }

        private void FocusOnObject()
        {
            if (addObject.placing)
            {
                return;
            }

            RaycastHit2D hit = Physics2D.Raycast(GetMousePos(), Vector2.zero);

            if (hit.transform == null && !editObject.editingObject)
            {
                StatusController.StatusMessage = "Simulating...";
                transform.parent = null;
                currentlyFocusedOn = null;
                focusing = false;
                return;
            }

            if (hit.transform != null)
            {
                MoveToClickedTarget(hit.transform);
                StatusController.StatusMessage = $"Focusing on {hit.transform.name}";
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

            // Tell the rest of the script that a planet has been focused
            focusing = true;


            // Set the inspector variable.
            currentlyFocusedOn = target.gameObject;
            //transform.position = new Vector3(target.position.x, target.position.y, -10);

            transform.parent = target;
        }

        void UpdateSmoothDampPos()
        {
            // Create the smooth camera pos based on the cameras current location and the focused target.
            smoothedCameraPos = Vector2.SmoothDamp(transform.position, currentlyFocusedOn.transform.position, ref posVelocity, amountOfSmoothing);
            // Move the camera to the new target, whilst keeping the z value intact
            transform.position = new Vector3(smoothedCameraPos.x, smoothedCameraPos.y, -10);
        }

        public Vector2 GetMousePos()
        {
            return mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        /// <summary>
        /// Checks to see whether the currently selected object is controllable.
        /// If it is, enable the UI and wait for input.
        /// </summary>
        public bool IsControllableObject()
        {
            if (currentlyFocusedOn == null)
            {
                return false;
            }
            // If we're currently focused on a controllable object
            if (currentlyFocusedOn.gameObject.tag == "Spaceship")
            {
                StatusController.StatusMessage = "Press \"E\" to control this object.";
                return true;
            }
            return false;
        }
    }
}
