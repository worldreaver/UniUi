#if UNITY_EDITOR
using UnityEditor;
using Worldreaver.EditorUtility;

namespace Worldreaver.UniUI
{
    [CustomEditor(typeof(UniButton), true)]
    [CanEditMultipleObjects]
    public class UniButtonEditor : UnityEditor.UI.ButtonEditor
    {
        private SerializedProperty _isMotion;
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
            _isMotion = EditorUtil.SerializeField(serializedObject, "isMotion");
            if (_isMotion.boolValue)
            {
                _isAffectToSelf = EditorUtil.SerializeField(serializedObject, "isAffectToSelf", "Affect To Self");
                if (!_isAffectToSelf.boolValue)
                {
                    EditorUtil.SerializeField(serializedObject, "affectObject", "Object Affect Transform");
                    EditorGUILayout.Space();
                }

                EditorUtil.SerializeField(serializedObject, "motionType");
                EditorUtil.SerializeField(serializedObject, "_motion");
            }

            Repaint();
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif