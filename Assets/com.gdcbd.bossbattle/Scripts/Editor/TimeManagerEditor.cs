using UnityEditor;
using UnityEngine;

namespace com.gdcbd.bossbattle.editor
{
    
    [CustomEditor(typeof(TimeManager))]
    public class TimeManagerEditor : UnityEditor.Editor
    {
        private GUIStyle headerStyle;

        private void OnEnable()
        {
            // Define the style for the H1 header
            headerStyle = new GUIStyle()
            {
                fontSize = 24,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState() { textColor = Color.white }
            };
        }

        public override void OnInspectorGUI()
        {
            // Get the target TimeManager instance
            TimeManager timeManager = (TimeManager)target;

            // Draw the H1 header for the _time variable
            EditorGUILayout.LabelField($"Time: {timeManager.TimeCount()}", headerStyle);

            // Call the base class method to draw the default inspector
            base.OnInspectorGUI();
        }
    }
}