using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mattordev.UI;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.Utils
{
    /// <summary>
    /// Allows the user to edit the parameters of an object in the simulation
    /// </summary>
    public class EditObject : MonoBehaviour
    {
        [Header("General")]
        public GameObject objectToEdit; // The object that's going to be edited
        public CameraController cameraController; // camera controller script   

        public GameObject editWindow; // Edit window UI

        // Get the selected objects colour
        SpriteRenderer spriteRenderer; // Used with the color picker
        public bool editingObject = false; // a check to see if we're editing an object

        [Header("UI Elements")]
        public TMP_InputField nameField;
        public Slider massSlider;
        public Slider speedSlider;
        public FlexibleColorPicker colorPicker;

        /// <summary>
        /// Fixed update, runs at a locked speed
        /// 
        /// Here we call the function to get the object that's being edited, and then call the functions to set the color and the sliders
        /// </summary>
        private void FixedUpdate()
        {
            GetObjectToEdit();

            if (objectToEdit != null && editingObject == true)
            {
                SetColor();
                SetSliders();
            }

            if (editWindow.activeInHierarchy == false)
            {
                editingObject = false;
            }
        }

        /// <summary>
        /// Turns on the edit window
        /// </summary>
        /// <param name="objectToEdit"></param>
        public void EnableEditWindow()
        {
            // Turn on the edit window
            editWindow.SetActive(true);
        }

        /// <summary>
        /// Gets the object to edit using  ray cast and then sets the sliders to the correct value (objects current value)
        /// </summary>
        public void GetObjectToEdit()
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (Input.GetMouseButtonDown(0) && hit.transform != null)
            {
                objectToEdit = hit.transform.gameObject;
                editingObject = true;

                // Set the parameters to match what is already set
                spriteRenderer = objectToEdit.GetComponent<SpriteRenderer>();
                colorPicker.SetColor(spriteRenderer.color);

                nameField.text = objectToEdit.name;
                Rigidbody2D rb2D = objectToEdit.GetComponent<Rigidbody2D>();

                massSlider.value += rb2D.mass;
                speedSlider.value += rb2D.velocity.magnitude;


                // Send status message
                StatusController.StatusMessage = $"Selected {hit.transform.name} for editing...";
            }
        }

        /// <summary>
        /// Sets the sliders to the correct value when the game starts
        /// </summary>
        public void SetSliders()
        {
            Rigidbody2D rb2D = objectToEdit.GetComponent<Rigidbody2D>();

            rb2D.mass = massSlider.value;
            // if (rb2D.velocity.x == 0)
            // {
            //     Vector2 newY = new Vector2(rb2D.velocity.x, speedSlider.value);
            //     rb2D.velocity = newY;
            // }
            // else
            // {
            //     Vector2 newX = new Vector2(speedSlider.value, rb2D.velocity.y);
            //     rb2D.velocity = newX;
            // }
        }

        /// <summary>
        /// Sets the color based on the color pickers color.
        /// </summary>
        public void SetColor()
        {
            // set the color from the color picker
            spriteRenderer.color = colorPicker.color;
        }

        /// <summary>
        /// Toggles object editing bool
        /// </summary>
        /// <param name="editing"></param>
        public void ToggleEditing(bool editing)
        {
            editingObject = editing;
        }
    }
}
