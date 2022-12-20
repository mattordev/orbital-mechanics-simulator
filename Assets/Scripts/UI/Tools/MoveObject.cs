using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mattordev.Universe;
using Mattordev.Utils.Stats;

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
        public bool moving = false;
        public GameObject selectedObject;

        public CameraController cameraController;
        public StatisticsTracker statisticsTracker;

        // Update is called once per frame
        void Update()
        {
            if (moving)
            {
                MoveObjectToMouse();
            }
        }

        void MoveObjectToMouse()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                selectedObject = cameraController.GetClickedItem();
                Debug.Log(selectedObject);
            }

            // Make sure that the selected object isn't nothing, also allow the player to cancel
            if (!selectedObject || Input.GetButtonDown("Fire2"))
            {
                // set moving to false to drop the planet at the current mouse pos
                moving = false;
                SimState(true);
                return;
            }

            selectedObject.transform.position = cameraController.GetMousePos();

            // Player clicks to drop the object
            if (Input.GetButtonDown("Fire2"))
            {
                // set moving to false to drop the planet at the current mouse pos
                moving = false;
                SimState(true);

                // Rigidbody2D rb2D = selectedObject.GetComponent<Rigidbody2D>();
                // rb2D.simulated = false;
            }

        }

        public void StartObjectMove()
        {
            moving = true;
        }

        /// <summary>
        /// Used for toggling the simulation without effecting Time.timeScale.
        /// </summary>
        /// <param name="state"></param>
        public void SimState(bool state)
        {
            Debug.Log($"Simulation state: {state}");

            // Disable Body simulation
            BodySimulation bodySimulation = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<BodySimulation>();
            bodySimulation.enabled = state;

            // Stop simulation on the bodies
            foreach (Attractor attractor in statisticsTracker.attractors)
            {
                Rigidbody2D rb2D = attractor.GetComponent<Rigidbody2D>();
                rb2D.simulated = state;
            }
        }
    }
}
