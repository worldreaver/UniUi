#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Worldreaver.EditorUtility;

namespace Worldreaver.UniUI
{
    [CustomEditor(typeof(UniProgressBar), true)]
    [CanEditMultipleObjects]
    public class UniProgressBarEditor : UnityEditor.Editor
    {
        protected SerializedProperty hasDelayBar;

        #region Overrides of Editor

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GlobalSetup();
            Draw();
            
            Repaint();
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void Draw()
        {
            EditorUtil.DrawSeparator(1.5f);
            EditorUtil.SerializeField(serializedObject, "direction");
            EditorUtil.SerializeField(serializedObject, "mode");
            hasDelayBar = EditorUtil.SerializeField(serializedObject, "hasDelayBar");

            EditorUtil.SerializeField(serializedObject, "foregroundBar");
            if (hasDelayBar.boolValue)
            {
                EditorUtil.SerializeField(serializedObject, "delayedBar");
            }

            GUILayout.Space(8);
            EditorUtil.SerializeField(serializedObject, "curveForegroundBar", "ForegroundBar Use Curve");

            if (hasDelayBar.boolValue)
            {
                EditorUtil.SerializeField(serializedObject, "curveDelayBar", "DelayBar Use Curve");
            }

            EditorUtil.SerializeField(serializedObject, "_foregroundMotion");
            if (hasDelayBar.boolValue)
            {
                EditorUtil.SerializeField(serializedObject, "_delaybarMotion");
            }
        }

        private void GlobalSetup()
        {
            GUILayout.Space(8);
            EditorUtil.DrawSeparator();
            var position = EditorGUILayout.GetControlRect();
            var current = serializedObject.FindProperty("current");
            var max = serializedObject.FindProperty("max");
            var barPosition = new Rect(position.position.x, position.position.y, position.size.x, EditorGUIUtility.singleLineHeight);
            var barLabel = $"{current.floatValue:0.00}" + "/" + max.floatValue;
            var fillPercentage = current.floatValue / max.floatValue;
            if (fillPercentage > 1)
            {
                GUI.contentColor = new Color(1f, 0.25f, 0.13f);
                var align = GUI.skin.label.alignment;
                GUI.skin.label.alignment = TextAnchor.UpperCenter;
                GUILayout.Space(8);
                GUI.Label(new Rect(position.position.x, position.position.y + 18, position.size.x, EditorGUIUtility.singleLineHeight), "Property value is over max value has been specified!");
                GUILayout.Space(8);
                GUI.contentColor = Color.white;
                GUI.skin.label.alignment = align;
            }

            DrawBar(barPosition, Mathf.Clamp01(fillPercentage), barLabel, new Color(0f, 0.62f, 0.82f), Color.white);
        }

        private static void DrawBar(Rect position, float fillPercent, string label, Color barColor, Color labelColor)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            var fillRect = new Rect(position.x, position.y, position.width * fillPercent, position.height);

            EditorGUI.DrawRect(position, new Color(0.4f, 0.45f, 0.5f));
            EditorGUI.DrawRect(fillRect, barColor);

            // set alignment and cache the default
            var align = GUI.skin.label.alignment;
            GUI.skin.label.alignment = TextAnchor.UpperCenter;

            // set the color and cache the default
            var c = GUI.contentColor;
            GUI.contentColor = labelColor;

            // calculate the position
            var labelRect = new Rect(position.x, position.y - 2, position.width, position.height);

            // draw~
            EditorGUI.DropShadowLabel(labelRect, label);

            // reset color and alignment
            GUI.contentColor = c;
            GUI.skin.label.alignment = align;
        }

        #endregion
    }
}
#endif

