using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        public GameObject objectToEdit;
        public CameraController cameraController;

        public GameObject editWindow;

        [Header("UI Elements")]
        public TMP_InputField nameField;
        public Slider massSlider;
        public Slider speedSlider;
        public FlexibleColorPicker colorPicker;


        private void FixedUpdate()
        {
            if (objectToEdit != null)
            {
                objectToEdit.transform.position = cameraController.GetMousePos();
            }
        }

        public void EditObjectParameters(GameObject objectToEdit)
        {
            // Turn on the edit window
            editWindow.SetActive(true);
        }

        public void SetSliders()
        {

        }
    }
}
