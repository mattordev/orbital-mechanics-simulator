
using UnityEditor.UI;
using UnityEditor;

namespace Mattordev.UI.Tools
{
    [CustomEditor(typeof(ButtonWithTooltip))]
    public class ButtonWithTooltipEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            ButtonWithTooltip component = (ButtonWithTooltip)target;

            component.panel = (TooltipPanel)EditorGUILayout.ObjectField("Tooltip Panel", component.panel, typeof(TooltipPanel), true);
            component.tooltip = EditorGUILayout.TextField("Tooltip", component.tooltip);

            base.OnInspectorGUI();
        }
    }
}