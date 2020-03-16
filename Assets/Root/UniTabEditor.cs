#if UNITY_EDITOR
using UnityEditor;
using Worldreaver.EditorUtility;

namespace Worldreaver.UniUI
{
    [CustomEditor(typeof(UniTap), true)]
    [CanEditMultipleObjects]
    public class UniTabEditor : UnityEditor.UI.ToggleEditor
    {
        private SerializedProperty _isMotion;
        private SerializedProperty _isAffectToSelf;
        private SerializedProperty _isSwitchSprite;
        private SerializedProperty _isExpan;
        private SerializedProperty _affectObject;
        private SerializedProperty _layoutElement;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawInspector();
        }

        protected virtual void DrawInspector()
        {
            EditorUtil.DrawSeparator();
            EditorUtil.SerializeField(serializedObject, "pivot");
            EditorUtil.SerializeField(serializedObject, "activeObjects");
            EditorUtil.SerializeField(serializedObject, "deactiveObjects");
            Draw();
        }

        protected void Draw()
        {
            _isSwitchSprite = EditorUtil.SerializeField(serializedObject, "isSwitchSprite");
            if (_isSwitchSprite.boolValue)
            {
                EditorUtil.SerializeField(serializedObject, "activeSprite");
                EditorUtil.SerializeField(serializedObject, "deactiveSprite");
                EditorGUILayout.Space();
            }

            _isMotion = EditorUtil.SerializeField(serializedObject, "isMotion");
            if (_isMotion.boolValue)
            {
                _isAffectToSelf = EditorUtil.SerializeField(serializedObject, "isAffectToSelf", "Affect To Self");
                if (!_isAffectToSelf.boolValue)
                {
                    _affectObject = EditorUtil.SerializeField(serializedObject, "affectObject", "Object Affect");
                    EditorGUILayout.Space();
                }
                else
                {
                    ClearAffect();
                }

                _isExpan = EditorUtil.SerializeField(serializedObject, "isExpan");
                if (_isExpan.boolValue)
                {
                    var isExpanLayout = EditorUtil.SerializeField(serializedObject, "isExpanLayout");
                    if (isExpanLayout.boolValue)
                    {
                        _layoutElement = EditorUtil.SerializeField(serializedObject, "layoutElement");
                        if (_layoutElement.objectReferenceValue != null)
                        {
                            EditorUtil.SerializeField(serializedObject, "valueExpand", "Value Expand");
                            EditorUtil.SerializeField(serializedObject, "valueFlexible", "Default Value Flexible");
                        }
                    }

                    if (!_isAffectToSelf.boolValue)
                    {
                        EditorUtil.SerializeField(serializedObject, "selectedScale", "Scale On Selected");
                        UnityEngine.GUI.enabled = false;
                        EditorUtil.SerializeField(serializedObject, "unSelectedScale", "Scale On UnSelected");
                        UnityEngine.GUI.enabled = true;
                    }

                    EditorGUILayout.Space();
                }
                else
                {
                    ClearLayoutElement();
                }

                EditorUtil.SerializeField(serializedObject, "motionType");
                EditorUtil.SerializeField(serializedObject, "_motion");
            }
            else
            {
                ClearAffect();
                ClearLayoutElement();
            }

            Repaint();
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }

        private void ClearAffect()
        {
            if (_affectObject != null)
            {
                _affectObject.objectReferenceValue = null;
            }
        }

        private void ClearLayoutElement()
        {
            if (_layoutElement != null)
            {
                _layoutElement.objectReferenceValue = null;
            }
        }
    }
}
#endif

