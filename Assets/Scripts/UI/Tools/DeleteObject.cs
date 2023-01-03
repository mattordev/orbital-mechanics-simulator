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
    /// Used for Deleting objects using the tools panel in the game.
    /// </summary>
    public class DeleteObject : MonoBehaviour
    {
        private CameraController cameraController;
        public bool deleting;
        public TableGenerator tableGenerator;

        // Start is called before the first frame update
        void Start()
        {
            cameraController = Camera.main.GetComponent<CameraController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (deleting)
            {
                GameObject toDelete = SelectObjectToDelete();
                cameraController.transform.parent = null;
                // Destroy the object
                Destroy(toDelete, .2f);
                // ?? Play animation/effect of object blowing up

                if (Input.GetMouseButtonDown(1))
                {
                    Debug.Log("clicked");
                    deleting = false;
                }
                // // Cleanup
                CleanUpAfterDeletion(toDelete);
            }
        }

        /// <summary>
        /// Fires out a ray and returns the gameobject that is hit AKA the gameobject to be deleted
        /// </summary>
        /// <returns>The hit gameobject if a suitable one is clicked, or nothing</returns>
        private GameObject SelectObjectToDelete()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                RaycastHit2D hit = Physics2D.Raycast(cameraController.GetMousePos(), Vector2.zero);
                // If the hit object has an attractor class on it
                if (hit != false)
                {
                    if (hit.transform.gameObject.GetComponent<Attractor>())
                    {
                        return hit.transform.gameObject;
                    }
                }
            }
            return null;
        }

        public void ToggleDeleting(bool deleting)
        {
            this.deleting = deleting;
        }

        #region Error Avoidance / Clean up
        public void CleanUpAfterDeletion(GameObject itemToCleanup)
        {

            Debug.ClearDeveloperConsole();

            tableGenerator.RegenerateTable();
            StatisticsTracker stats = FindObjectOfType<StatisticsTracker>();
            BodySimulation bodySimulation = FindObjectOfType<BodySimulation>();


            // Clear the bodies list and get it again
            // If this is uncommented the bodies will just go straight and not follow thier orbits
            // if it's uncommented there's an error but that's it.
            bodySimulation.GetBodies();
            stats.attractors.Clear();
            stats.GetBodies();
            cameraController.focusing = false;
        }
        #endregion
    }
}
