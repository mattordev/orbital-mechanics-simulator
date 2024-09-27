using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mattordev.Universe;
using Mattordev.Utils.Stats;
using Mattordev.UI;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.Utils
{
    /// <summary>
    /// Class for moving selected objects around the scene
    /// </summary>
    public class MoveObject : MonoBehaviour
    {
        [Header("General")]
        public bool moving = false; // check to see whether we're moving an object
        public GameObject selectedObject; // the object that has been selected

        public Camera mainCam; // main camera
        public CameraController cameraController; // camera controller script

        // simtate pausing
        public StatisticsTracker statisticsTracker;

        List<Vector2> savedVelocities = new List<Vector2>();
        long prevFixedDeltaTime;

        private void Start()
        {
            mainCam = Camera.main;
        }

        /// <summary>
        /// FixedUpdate, runs at a locked speed.
        /// 
        /// If we're moving an object and there's a selected object set move it to the mouse position.
        /// otherwise call the function to move the object to the mouse.
        /// </summary>
        void FixedUpdate()
        {
            if (moving)
            {
                SimState(false);
                if (selectedObject != null)
                {
                    selectedObject.transform.position = cameraController.GetMousePos();
                }
                MoveObjectToMouse();
            }
            else
            {
                SimState(true);
            }
        }

        /// <summary>
        /// Tells the script that we're moving an object. 
        /// 
        /// Updates the status too.
        /// </summary>
        public void StartObjectMove()
        {
            moving = true;
            StatusController.StatusMessage = "Pick an object to move...";
        }

        /// <summary>
        /// Move the body to the mouse using a raycast. Uses left click to select, right click to drop.
        /// 
        /// Might be cool to add smoothdamp, or make the tool a toggle so it toggles move mode and anything can be moved.
        /// </summary>
        void MoveObjectToMouse()
        {
            Vector2 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform != null)
                {
                    selectedObject = hit.transform.gameObject;
                    StatusController.StatusMessage = $"Moving {hit.transform.name}";
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                StatusController.StatusMessage = $"No longer moving body...";
                moving = false;
                selectedObject = null;
            }
        }

        /// <summary>
        /// Used for toggling the simulation without effecting Time.timeScale.
        /// 
        /// TODO: FIGURE OUT HOW TO FREEZE RIGIBODIES WHILST THEY STAY SIMULATED OTHERWISE RAYCASTS DONT WORK
        /// </summary>
        /// <param name="state"></param>
        public void SimState(bool state)
        {
            Debug.Log($"Simulation state: {state}");

            // Disable Body simulation
            BodySimulation bodySimulation = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<BodySimulation>();
            bodySimulation.enabled = state;

            if (state == true)
            {
                UnfreezePosition();
            }
            else
            {
                FreezePosition();
            }

            // Stop simulation on the bodies
            // foreach (Attractor attractor in statisticsTracker.attractors)
            // {
            //     attractor.enabled = state;
            // }
        }

        void FreezePosition()
        {
            foreach (Attractor attractor in statisticsTracker.attractors)
            {
                Rigidbody2D rb2D = attractor.GetComponent<Rigidbody2D>();
                savedVelocities.Add(rb2D.velocity);
                // Set the attractor's current velocity to 0
                rb2D.velocity = Vector2.zero;
                rb2D.isKinematic = true;
            }
        }
        /// <summary>
        /// Unfreeze the position of the attractors
        /// </summary>
        void UnfreezePosition()
        {
            int i = 0;
            foreach (Attractor attractor in statisticsTracker.attractors)
            {
                Rigidbody2D rb2D = attractor.GetComponent<Rigidbody2D>();
                if (i < savedVelocities.Count)
                {
                    rb2D.velocity = savedVelocities[i];
                    rb2D.isKinematic = false;
                }
                i++;
            }
            savedVelocities.Clear();
        }
    }
}
