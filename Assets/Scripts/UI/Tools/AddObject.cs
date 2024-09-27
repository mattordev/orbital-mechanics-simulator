using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mattordev.Universe;
using Mattordev.UI;
using Mattordev.Utils.Stats;
using TMPro;
using UnityEngine.SceneManagement;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.Utils
{
    /// <summary>
    /// Used for Adding objects using the tools panel to the game.
    /// </summary>
    public class AddObject : MonoBehaviour
    {
        GameObject selectedObject;
        public List<GameObject> objectPrefabs;

        // placing vars
        float prevTimeScale;
        public bool placing;
        private Vector2 smoothedPlacingPos;
        private Vector2 posVelocity;
        public float amountOfSmoothing;
        private ObjectData objectData;
        public UtilityUIController utilityUIController;
        public CameraController camController;
        public StatisticsTracker statisticsTracker;

        // orbit display
        public OrbitDisplay orbitDisplay;

        // UI
        public GameObject velocityUI;
        private TMP_InputField inputField;

        private void Start()
        {
            // Find the camera controller
            camController = Camera.main.GetComponent<CameraController>();
        }

        /// <summary>
        /// Checks to see mainly if there is a new body being placed, if there is, handle the logic
        /// like pausing the sim and amking sure everything stays in the correct place.
        /// </summary>
        void Update()
        {
            // If an object is being placed..
            if (placing)
            {
                utilityUIController.additionWindow.SetActive(false);
                // Call the function to check if the pos of a placed object needs updating.
                // UpdateSmoothDampPos();
                selectedObject.transform.position = camController.GetMousePos();

                // Player clicks to drop the object
                if (Input.GetMouseButtonDown(0))
                {
                    // set placing to false to drop the planet at the current mouse pos
                    placing = false;

                    StatusController.StatusMessage = "Type in an initial Velocity (e.g 500)...";

                    // Disable simulation untill its speed has been set
                    Rigidbody2D rb2D = selectedObject.GetComponent<Rigidbody2D>();
                    rb2D.simulated = false;
                }

                // Enable orbit display -- DOESNT WORK
                orbitDisplay.enabled = true;
                orbitDisplay.hideOrbitPathsOnPlay = false;
                orbitDisplay.useThickLines = true;

                // enable UI to enter speed ?? enable drag to scale speed?
                velocityUI.SetActive(true);
            }
        }


        /// <summary>
        /// used for setting object data on click. Mainly used by other scripts.
        /// </summary>
        /// <param name="selectedObj">the selected body</param>
        public void SelectedObject(GameObject selectedObj)
        {
            Debug.Log("Setting object");
            selectedObject = selectedObj;
            objectData = selectedObj.GetComponent<ObjectData>();
        }

        /// <summary>
        /// the core function for placing objects. 
        /// 
        /// Pauses the simulation and then creates a new body either based on the users selection, or a base one.
        /// </summary>
        public void PlaceObject()
        {
            // Pause sim
            SimState(false);

            // User selects an object
            if (!selectedObject)
            {
                // place the default base planet
                InstantiateObject(0);
            }
            // Spawn the object
            GameObject instantiatedGO = InstantiateObject(objectData.objNumber);
            if (objectData.objNumber > objectPrefabs.Count)
            {
                Debug.Log("Not implemented yet");
                // Update status to say that the object isn't avaiable.
            }

            // Set public variable to the same as instantaited one.
            selectedObject = instantiatedGO;


            StatusController.StatusMessage = "Type in an initial Velocity (e.g 500)...";

            Rigidbody2D rb2D = selectedObject.GetComponent<Rigidbody2D>();
            rb2D.simulated = false;

            // Tell the script we're now placing a planet.
            placing = true;
        }

        // Currently doesn't set velocity correctly.
        // 02.01.23 - I finally figured it out! it's because the planet is *techinically* being spawned in with
        // a velocity of zero, adding the speed to the "initial velocty" after instantiating it would never work.
        /// <summary>
        /// Sets the initial vel of a body. Needs more work to work properly.
        /// </summary>
        public void SetInitialVelocity()
        {
            inputField = velocityUI.GetComponentInChildren<TMP_InputField>();
            Attractor currentAttractor = selectedObject.GetComponent<Attractor>();
            currentAttractor.initialVelocity.y = int.Parse(inputField.text);
            currentAttractor.currentVelocity = currentAttractor.initialVelocity;



            // Unpause sim 
            SimState(true);
            currentAttractor.UpdatePosition(statisticsTracker.universeParameters.physicsTimeStep);
        }

        /// <summary>
        /// Used for toggling the simulation without effecting Time.timeScale.
        /// </summary>
        /// <param name="state"></param>
        void SimState(bool state)
        {
            Debug.Log($"Simulation state: {state}");
            statisticsTracker.GetBodies();
            statisticsTracker.updateStats = state;

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

        /// <summary>
        /// Instantiates a body with the correct postion, and rotation based on the prefab number
        /// </summary>
        /// <param name="prefabNumber">The prefab that is to be spawned.</param>
        /// <returns></returns>
        GameObject InstantiateObject(int prefabNumber)
        {
            return Instantiate(objectPrefabs[prefabNumber], Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// Updates the cameras postion to move it through using Smooth Damp
        /// </summary>
        void UpdateSmoothDampPos()
        {
            // If we're not placing, return out of the function.
            if (!placing)
            {
                return;
            }
            CameraController cameraController = Camera.main.GetComponent<CameraController>();
            // Create the smooth camera pos based on the cameras current location and the focused target.
            smoothedPlacingPos = Vector2.SmoothDamp(selectedObject.transform.position, cameraController.GetMousePos(), ref posVelocity, amountOfSmoothing);
            // Move the camera to the new target, whilst keeping the z value intact
            transform.position = new Vector3(smoothedPlacingPos.x, smoothedPlacingPos.y, -10);
        }
    }
}
