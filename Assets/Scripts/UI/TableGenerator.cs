using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mattordev.Universe;
using Mattordev.Utils;
using UnityEngine.UI;

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
                    // Populate the item fields by passing in the needed params
                    PopulateItemFields(item, attractor, attractor.GetComponent<SpriteRenderer>(), attractor.name, attractor.GetComponent<Rigidbody2D>().mass, attractor.GetComponent<Rigidbody2D>().velocity.y);
                }
                FindTableItems();
                hasTableBeenSetup = true;
                Debug.Log($"Table has been setup. Added {tableItems.Count} items to the table.");
            }
            Debug.Log("Table has already been setup.");
        }

        #region Table Item Getting/Cleaning

        /// <summary>
        /// Finds all the table items in the scene if there is any
        /// </summary>
        void FindTableItems()
        {
            // Clear the list to avoid duplication
            tableItems.Clear();

            foreach (ItemInfoHolder item in FindObjectsOfType<ItemInfoHolder>())
            {
                // Add the current iterator.
                tableItems.Add(item.gameObject);
            }
        }

        void ClearTableItems()
        {
            // Find table items
            FindTableItems();

            // Make sure that the tables items isn't zero
            if (tableItems.Count == 0)
            {
                return;
            }

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
            attractors.Clear();
            foreach (Attractor body in FindObjectsOfType<Attractor>())
            {
                attractors.Add(body);
            }
            Debug.Log($"Sucessfully found {attractors.Count} bodies");
        }


        /// <summary>
        /// Populates the item fields with the correct parameters. 
        /// </summary>
        /// <param name="item"> Item object, used for getting reference to the ItemTextHolder.</param>
        /// /// <param name="attractor"> Attractor, used for focusing later.</param>
        /// <param name="name"> The name of the body.</param>
        /// <param name="mass"> The mass of the body.</param>
        /// <param name="speed"> The speed of the body</param>
        void PopulateItemFields(GameObject item, Attractor attractor, SpriteRenderer image, string name, float mass, float speed)
        {
            ItemInfoHolder itemTextHolder = item.GetComponent<ItemInfoHolder>();
            itemTextHolder.attractor = attractor;

            itemTextHolder.image.sprite = image.sprite;

            itemTextHolder.nameText.text = name;
            itemTextHolder.massText.text = mass.ToString();
            // Need to get these to update dynamically as the speed changes
            itemTextHolder.speedText.text = speed.ToString();
        }

        /// <summary>
        /// Functions for the buttons, facilitates focusing on bodies that are listed in the table.
        /// </summary>
        public void TableClickToFocus(GameObject button)
        {
            ItemInfoHolder itemTextHolder = button.GetComponent<ItemInfoHolder>();
            cameraController.MoveToClickedTarget(itemTextHolder.attractor.gameObject.transform);
        }

        #endregion
    }
}
