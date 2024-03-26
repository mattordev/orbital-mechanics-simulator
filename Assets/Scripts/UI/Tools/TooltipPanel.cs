using UnityEngine;
using TMPro;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.UI.Tools
{
    public class TooltipPanel : MonoBehaviour
    {
        private TMP_Text textObj;
        private RectTransform rect;

        public string description = string.Empty;

        public bool showHoverPanel;


        /// <summary>
        /// Calls the setup function
        /// </summary>
        void Start()
        {
            Setup();
        }

        /// <summary>
        /// This sets up the script for proper usage.
        /// makes sure that the tooltip starts disabled, and gets reference to the rect
        /// 
        /// it then calls the SetData() func to properly set the data in the tooltip.
        /// </summary>
        void Setup()
        {
            gameObject.SetActive(false);
            rect = GetComponent<RectTransform>();
            SetData();
        }

        /// <summary>
        /// Sets the "data" or the string in the tooltip.
        /// 
        /// note: might be worth just inverting the error catch in here to make it a guard clause. It might look a little cleaner.
        /// the only thing the current solution does is make sure that if NO description is passed in, it sets a default one.
        /// </summary>
        public void SetData()
        {
            textObj = GetComponentInChildren<TMP_Text>();

            if (description.Length != 0)
            {
                textObj.text = description;
            }
            description = "This is a description of the tool";
            textObj.text = description;
        }

        /// <summary>
        /// Used for checking whether the button is focused
        /// 
        /// from what I can tell, unneeded, will be removed in a few commits after testing
        /// </summary>
        /// <param name="buttonHighlighted"></param>
        /// <returns></returns>
        // public bool OnFocusChanged(bool buttonHighlighted)
        // {
        //     EnableTooltip(buttonHighlighted);
        //     return true;
        // }

        /// <summary>
        /// Used for setting the mouse position variables and setting the position of the tooltip.
        ///
        /// Early return if the panel isn't showing.
        /// </summary>
        private void Update()
        {
            if (!showHoverPanel)
            {
                return;
            }
            Vector3 offset = (Vector3)rect.sizeDelta / 2;
            Vector3 mousePos = Input.mousePosition;
            transform.position = mousePos += offset;
        }

        /// <summary>
        /// Convience method, allows us to enable the tooltip with no string.
        /// </summary>
        /// <param name="enabled"></param>
        public void EnableTooltip(bool enabled)
        {
            EnableTooltip(enabled, string.Empty);
        }

        /// <summary>
        /// Enables and disables the tooltip, also used for setting the text of the tooltip too.
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="text"></param>
        public void EnableTooltip(bool enabled, string text)
        {
            showHoverPanel = enabled;
            textObj.text = text;
            gameObject.SetActive(enabled);
        }
    }
}
