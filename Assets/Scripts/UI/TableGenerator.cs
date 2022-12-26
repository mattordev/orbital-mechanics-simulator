using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mattordev.Universe;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.UI
{
    /// <summary>
    /// Populates the table properly based on how many bodies there are in the scene.
    /// </summary>
    public class TableGenerator : MonoBehaviour
    {
        public List<GameObject> tableItems;
        public List<Attractor> attractors;
        public GameObject TableItemPrefab;
        public GameObject content;

        public bool hasTableBeenSetup;

        // Start is called before the first frame update
        void Start()
        {
            SetupTable();
        }

        /// <summary>
        /// Sets up the table and populates the items fields. Calls other methods.
        /// </summary>
        public void SetupTable()
        {
            if (hasTableBeenSetup != true)
            {
                ClearTableItems();
                GetAttractors();

                foreach (Attractor attractor in attractors)
                {
                    // Spawns an empty item prefab
                    GameObject item = Instantiate(TableItemPrefab, content.transform);
                    // Set the item to be a child of content gameobject
                    item.transform.SetParent(content.transform, false);
                    Debug.Log($"Spawning {item.name}");
                    // Populate the item fields by passing in the needed params
                    PopulateItemFields(item, attractor.name, attractor.GetComponent<Rigidbody2D>().mass, attractor.GetComponent<Rigidbody2D>().velocity.y);
                }
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
            foreach (ItemTextHolder item in FindObjectsOfType<ItemTextHolder>())
            {
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
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="name"></param>
        /// <param name="mass"></param>
        /// <param name="speed"></param>
        void PopulateItemFields(GameObject item, string name, float mass, float speed)
        {
            ItemTextHolder itemTextHolder = item.GetComponent<ItemTextHolder>();

            itemTextHolder.nameText.text = name;
            itemTextHolder.massText.text = mass.ToString();
            itemTextHolder.speedText.text = speed.ToString();
        }
    }
}
