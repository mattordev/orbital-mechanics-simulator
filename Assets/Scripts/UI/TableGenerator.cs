using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mattordev.Universe;
using Mattordev.Utils;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.UI
{
    public class TableItem
    {
        public GameObject tableItem { get; set; }
        public Attractor attractor { get; set; }
    }

    /// <summary>
    /// Populates the table properly based on how many bodies there are in the scene.
    /// </summary>
    public class TableGenerator : MonoBehaviour
    {
        public List<GameObject> tableItems; // The items in the table
        public List<Attractor> attractors; // The list of bodies in the scene
        // public List<TableItem> tableItems
        public GameObject TableItemPrefab; // The UI prefab to instantiate
        public GameObject content; // Scroll view content

        public bool hasTableBeenSetup; // Check to say whether the table has been setup or not

        Camera mainCam;
        CameraController cameraController;

        // Start is called before the first frame update
        void Start()
        {
            mainCam = Camera.main;
            cameraController = mainCam.GetComponent<CameraController>();

            // Call the setup.
            SetupTable();
        }

        /// <summary>
        /// Sets up the table and populates the items fields. Calls other methods.
        /// </summary>
        public void SetupTable()
        {
            if (hasTableBeenSetup != true)
            {
                // Find the attractors in scene
                GetAttractors();

                foreach (Attractor attractor in attractors)
                {
                    // Spawns an empty item prefab
                    GameObject item = Instantiate(TableItemPrefab, content.transform);
                    // Set the item to be a child of content gameobject
                    item.transform.SetParent(content.transform, false);
                    Debug.Log($"Spawning {item.name}");
                    // Get the info holder, pass in the attractor and call the SetInfo Function
                    ItemInfoHolder info = item.GetComponent<ItemInfoHolder>();
                    info.SetInfo(attractor);
                }
                // Find the table items in the scene, add them to the list.
                FindTableItems();
                // Mark the table as setup to avoid it being run again.
                hasTableBeenSetup = true;
                Debug.Log($"Table has been setup. Added {tableItems.Count} items to the table.");
            }
            Debug.Log("Table has already been setup.");
        }

        #region Table Item Getting/Cleaning

        /// <summary>
        /// Clears the tableItems list, 
        /// then it finds all the table items in the scene if there is any, adds them to the tableItems list.
        /// </summary>
        void FindTableItems()
        {
            // Clear the list to avoid duplication
            tableItems.Clear();

            // For each info holder that can be found in the scene
            foreach (ItemInfoHolder item in FindObjectsOfType<ItemInfoHolder>())
            {
                // Add the current iterator.
                tableItems.Add(item.gameObject);
            }
        }

        /// <summary>
        /// Finds all table items, and then removes them from the scene/clears the table.
        /// </summary>
        void ClearTableItems()
        {
            // Find table items
            FindTableItems();

            // Make sure that the tables items isn't zero
            if (tableItems.Count == 0)
            {
                return;
            }

            // For each item that is in tableItems list
            foreach (GameObject item in tableItems)
            {
                // Remove item from the list
                tableItems.Remove(item);
                // Destroy the item obj
                Destroy(item);
            }
        }

        #endregion

        #region Other Functions

        /// <summary>
        /// Finds all the attractors/bodies/planets
        /// </summary>
        public void GetAttractors()
        {
            // Clear the attractors list
            attractors.Clear();
            // For each attractor script in the scene
            foreach (Attractor body in FindObjectsOfType<Attractor>())
            {
                // Add the current iterator to the list
                attractors.Add(body);
            }
            Debug.Log($"Sucessfully found {attractors.Count} bodies");
        }

        /// <summary>
        /// Functions for the buttons, facilitates focusing on bodies that are listed in the table.
        /// </summary>
        public void TableClickToFocus(GameObject button)
        {
            // Get reference to the info holder
            ItemInfoHolder itemTextHolder = button.GetComponent<ItemInfoHolder>();
            // Call the camera move
            cameraController.MoveToClickedTarget(itemTextHolder.attractor.gameObject.transform);
        }

        #endregion
    }
}
