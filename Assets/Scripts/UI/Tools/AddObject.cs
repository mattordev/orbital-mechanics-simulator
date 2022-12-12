using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mattordev.Universe;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.Utils
{
    public class AddObject : MonoBehaviour
    {
        GameObject selectedObject;
        public List<GameObject> objectPrefabs;

        // placing vars
        float prevTimeScale;
        bool placing;
        private Vector2 smoothedPlacingPos;
        private Vector2 posVelocity;
        public float amountOfSmoothing;

        // orbit display
        public OrbitDisplay orbitDisplay;

        // UI
        public GameObject velocityUI;

        // Update is called once per frame
        void Update()
        {
            // Call the function to check if the pos of a placed object needs updating.
            UpdateSmoothDampPos();

            // If an object is being placed..
            if (placing)
            {
                // Player clicks to drop the object
                if (Input.GetMouseButtonDown(2))
                {
                    // set placing to false to drop the planet at the current mouse pos
                    placing = false;
                }

                // Enable orbit display
                orbitDisplay.enabled = true;
                orbitDisplay.hideOrbitPathsOnPlay = false;

                // enable UI to enter speed ?? enable drag to scale speed?
                velocityUI.SetActive(true);

                // enable attractor.cs
                Attractor attractor = selectedObject.GetComponent<Attractor>();
                attractor.enabled = true;

                // Unpause sim 
                Time.timeScale = prevTimeScale;
                // Set placing to false
                placing = false;
            }
        }

        public void PlaceObject()
        {
            // Pause sim
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0;

            // User selects an object
            if (!selectedObject)
            {
                // place the default base planet
            }

            // Get the selected object number VV this line doesn't work
            int objectNumber = selectedObject.transform.parent.transform.hierarchyCount;
            // Spawn the object, minus one as the prefabs start at zero
            GameObject instantiatedGO = InstantiateObject(objectNumber - 1);

            // after clicked, object follows the mouse
            // Get ref to camera to get mouse pos
            CameraController cameraController = Camera.main.GetComponent<CameraController>();
            // Set public variable to the same as instantaited one.
            selectedObject = instantiatedGO;
            // Tell the script we're now placing a planet.
            placing = true;
        }

        GameObject InstantiateObject(int prefabNumber)
        {
            return Instantiate(objectPrefabs[prefabNumber], Vector3.zero, Quaternion.identity);
        }

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
