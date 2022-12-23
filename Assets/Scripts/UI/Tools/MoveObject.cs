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

        public Camera mainCam;
        public CameraController cameraController;
        public StatisticsTracker statisticsTracker;

        private void Start()
        {
            mainCam = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            if (moving)
            {
                MoveObjectToMouse();
            }
        }

        public void StartObjectMove()
        {
            moving = true;
            SimState(false);
        }

        void MoveObjectToMouse()
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (Input.GetMouseButtonDown(0) && hit.transform != null)
            {
                selectedObject = hit.transform.gameObject;

                Debug.Log(selectedObject);
                SimState(true);
                selectedObject.transform.position = cameraController.GetMousePos();
            }

            // Player clicks to drop the object
            if (Input.GetMouseButtonDown(1))
            {
                // set moving to false to drop the planet at the current mouse pos
                moving = false;
                SimState(true);
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
            foreach (Attractor attractor in statisticsTracker.attractors)
            {
                attractor.enabled = state;
            }
        }

        // Freeze the position of the game object
        void FreezePosition()
        {

        }

        // Reapply the saved velocity
        void UnfreezePosition()
        {

        }
    }
}
