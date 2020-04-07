#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Worldreaver.EditorUtility;

namespace Worldreaver.UniUI
{
    [CustomEditor(typeof(UniButton), true)]
    [CanEditMultipleObjects]
    public class UniButtonEditor : UnityEditor.UI.ButtonEditor
    {
        private SerializedProperty _isMotion;
        private SerializedProperty _allowMotionDisable;
        private SerializedProperty _isAffectToSelf;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawInspector();
        }

        protected virtual void DrawInspector()
        {
            EditorUtil.DrawSeparator();
            EditorUtil.SerializeField(serializedObject, "pivot");
            Draw();
        }

        protected void Draw()
        {
            GUILayout.Space(6);
            _isMotion = EditorUtil.SerializeField(serializedObject, "isMotion");
            if (_isMotion.boolValue)
            {
                _isAffectToSelf = EditorUtil.SerializeField(serializedObject, "isAffectToSelf", "Affect To Self");
                if (!_isAffectToSelf.boolValue)
                {
                    EditorUtil.SerializeField(serializedObject, "affectObject", "Object Affect Transform");
                    EditorGUILayout.Space();
                }

                _allowMotionDisable = EditorUtil.SerializeField(serializedObject, "allowMotionDisable", "Allow Motion When Disable");

                GUILayout.Space(6);
                EditorUtil.SerializeField(serializedObject, "motionType", "Type Motion");
                EditorUtil.SerializeField(serializedObject, "_motion", "Motion Data");
                if (_allowMotionDisable.boolValue)
                {
                    GUILayout.Space(8);
                    EditorUtil.SerializeField(serializedObject, "motionTypeDisable", "Type Motion When Disable");
                    EditorUtil.SerializeField(serializedObject, "_motionDisable", "Motion Data");
                }
            }

            Repaint();
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif