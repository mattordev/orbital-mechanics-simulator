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
    /// <summary>
    /// This class is used to focus the camera on a random object in the scene, used for the main menu, creating a more
    /// interactive and dynamic experience.
    /// </summary>
    public class RandomFocusser : MonoBehaviour
    {
        [Header("Random settings")]
        public bool randomFocus = true; // Defines whether the script should focus on a random object
        public float focusTimeMin = 5f; // The minimum time it takes to focus on a new object
        public float focusTimeMax = 15f; // The maximum time it takes to focus on a new object
        public float zoomSpeed = 3f; // The speed at which the camera zooms in and out
        bool isCoroutineRunning = false; // Defines whether the coroutine is currently running

        [Header("References")]
        public CameraController cameraController; // Reference to the camera controller

        // Update is called once per frame
        void Update()
        {
            // start the coroutine to focus on a random object
            if (randomFocus && !isCoroutineRunning)
            {
                StartCoroutine(FocusOnRandomObject());
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public void StopRandomFocus()
        {
            randomFocus = false;
        }

        /// <summary>
        /// Uses the BodySimulation to get a random object in the scene from its attractors list
        /// </summary>
        public GameObject GetRandomObject()
        {
            BodySimulation bodySimulation = FindObjectOfType<BodySimulation>();
            if (bodySimulation != null && bodySimulation.bodies.Length > 0)
            {
                int randomIndex = Random.Range(0, bodySimulation.bodies.Length);
                return bodySimulation.bodies[randomIndex].gameObject;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// gets a random zoom level for the camera to focus on, lerps the camera to the new zoom level.
        /// </summary>
        /// <returns>The zoom level</returns>
        public float GetRandomZoomLevel()
        {
            return Random.Range(cameraController.minScale, cameraController.maxScale);
        }


        /// <summary>
        /// Coroutine that focuses on a random object in the scene, this coroutine calls itself after the wait time
        /// </summary>
        /// <returns></returns>
        public IEnumerator FocusOnRandomObject()
        {
            isCoroutineRunning = true;

            yield return new WaitForSeconds(Random.Range(focusTimeMin, focusTimeMax));

            isCoroutineRunning = false;
            cameraController.currentlyFocusedOn = GetRandomObject();
            Debug.Log("Focusing on random object");

            if (randomFocus)
            {
                StartCoroutine(FocusOnRandomObject());
                StartCoroutine(ChangeZoomLevel(Random.Range(focusTimeMin, focusTimeMax), zoomSpeed));
            }
        }

        public IEnumerator ChangeZoomLevel(float time, float zoomSpeed)
        {
            float startZoomLevel = cameraController.currentScale;
            float targetZoomLevel = GetRandomZoomLevel();
            float elapsedTime = 0f;

            while (elapsedTime < time)
            {
                float zoomLevel = Mathf.Lerp(startZoomLevel, targetZoomLevel, elapsedTime / time);
                cameraController.currentScale = zoomLevel;

                elapsedTime += Time.deltaTime * zoomSpeed;
                yield return null;
            }

            cameraController.currentScale = targetZoomLevel;
        }
    }
}
