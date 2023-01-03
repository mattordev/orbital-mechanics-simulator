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
namespace Mattordev
{
    public class UpdateTextBasedOnSliderValue : MonoBehaviour
    {
        public Slider slider;
        public TMP_Text text;

        // Update is called once per frame
        void Update()
        {
            text.text = slider.value.ToString();
        }
    }
}
