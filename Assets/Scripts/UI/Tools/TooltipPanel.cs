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


        // Start is called before the first frame update
        void Start()
        {
            Setup();
        }

        /// <summary>
        /// This sets up the script for proper usage.
        /// </summary>
        void Setup()
        {
            // base.textObj = hoverPanelText;
            // base.SetData();

            gameObject.SetActive(false);
            rect = GetComponent<RectTransform>();
            SetData();
        }

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

        public bool OnFocusChanged(bool buttonHighlighted)
        {
            EnableTooltip(buttonHighlighted);
            return true;
        }

        private void Update()
        {
            if (!showHoverPanel)
            {
                return;
            }

            // Get the mouse position
            // Vector3 rectSize =
            Vector3 offset = (Vector3)rect.sizeDelta / 2;
            Vector3 mousePos = Input.mousePosition;
            transform.position = mousePos += offset;
        }

        public void EnableTooltip(bool enabled)
        {
            EnableTooltip(enabled, string.Empty);
        }

        public void EnableTooltip(bool enabled, string text)
        {
            showHoverPanel = enabled;
            textObj.text = text;
            gameObject.SetActive(enabled);
        }
    }
}
