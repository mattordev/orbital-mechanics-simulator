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
                if (selectedObject != null)
                {
                    selectedObject.transform.position = cameraController.GetMousePos();
                }
                MoveObjectToMouse();
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
            // Fire a ray from the camera to the mouse pos
            RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            // if the player clicks on an object, set the selectedObject as the clicked obj and update the status
            if (Input.GetMouseButtonDown(0) && hit.transform != null)
            {
                selectedObject = hit.transform.gameObject;

                StatusController.StatusMessage = $"Moving {hit.transform.name}";
            }

            // Player clicks to drop the object
            if (Input.GetMouseButtonDown(1))
            {
                // set moving to false to drop the planet at the current mouse pos
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
        // public void SimState(bool state)
        // {
        //     Debug.Log($"Simulation state: {state}");

        //     // Disable Body simulation
        //     BodySimulation bodySimulation = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<BodySimulation>();
        //     bodySimulation.enabled = state;

        //     if (state == true)
        //     {
        //         UnfreezePosition();
        //     }
        //     else
        //     {
        //         FreezePosition();
        //     }

        //     // Stop simulation on the bodies
        //     foreach (Attractor attractor in statisticsTracker.attractors)
        //     {
        //         attractor.enabled = state;
        //     }
        // }

        // // Freeze the position of the game objects
        // void FreezePosition()
        // {
        //     prevFixedDeltaTime = (long)Time.fixedDeltaTime;
        //     Time.fixedDeltaTime = 0;
        //     foreach (Attractor attractor in statisticsTracker.attractors)
        //     {
        //         Rigidbody2D rb2D = attractor.GetComponent<Rigidbody2D>();
        //         savedVelocities.Add(rb2D.velocity);
        //         rb2D.velocity = Vector2.zero;

        //     }
        // }

        // // Reapply the saved velocity
        // void UnfreezePosition()
        // {
        //     Time.fixedDeltaTime = prevFixedDeltaTime;
        //     foreach (Attractor attractor in statisticsTracker.attractors)
        //     {
        //         foreach (Vector2 velocity in savedVelocities)
        //         {
        //             Rigidbody2D rb2D = attractor.GetComponent<Rigidbody2D>();
        //             rb2D.velocity = velocity;
        //         }
        //     }
        // }
    }
}
