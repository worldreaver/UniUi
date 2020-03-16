#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Worldreaver.EditorUtility;


namespace Worldreaver.UniUI
{
    [CustomEditor(typeof(UniProgressBarTMP), true)]
    [CanEditMultipleObjects]
    // ReSharper disable once InconsistentNaming
    public class UniProgressBarTMPEditor : UniProgressBarEditor
    {
        protected override void Draw()
        {
            EditorUtil.DrawSeparator(1.5f);
            EditorUtil.SerializeField(serializedObject, "direction");
            EditorUtil.SerializeField(serializedObject, "mode");
            hasDelayBar = EditorUtil.SerializeField(serializedObject, "hasDelayBar");
            EditorUtil.SerializeField(serializedObject, "text");
            GUILayout.Space(2);
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
    }
}
#endif

