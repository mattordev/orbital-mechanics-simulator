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

        public void Start()
        {
            Application.focusChanged += () => panel.OnFocusChanged(IsHighlighted());
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            panel.EnableTooltip(true, tooltip);
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            panel.EnableTooltip(false);
            base.OnPointerExit(eventData);
        }
    }
}
