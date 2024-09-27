using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.UI.Tools
{
    /// <summary>
    /// A simple class for holding information about the tool.
    /// </summary>
    public class ButtonWithTooltip : Button
    {
        public TooltipPanel panel;
        public string tooltip;
        public bool windowFocused;

        // Might need this in the future, can't remember what it was for specifically so leaving it in for now
        // public void Start()
        // {
        //     //Application.focusChanged += () => panel.OnFocusChanged(IsHighlighted());
        // }

        /// <summary>
        /// Base class for when the pointer hovers over the button
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerEnter(PointerEventData eventData)
        {
            panel.EnableTooltip(true, tooltip);
            base.OnPointerEnter(eventData);
        }

        /// <summary>
        /// Base class for when the pointer exits hovering over the button
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerExit(PointerEventData eventData)
        {
            panel.EnableTooltip(false);
            base.OnPointerExit(eventData);
        }
    }
}
